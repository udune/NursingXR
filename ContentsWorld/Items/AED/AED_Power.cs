using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class AED_Power : Interaction
{
    [SerializeField] AED aed;
    [SerializeField] private AED_Handle handle;
    private float value;
    public float target;
    public bool on;
    private AudioSource audio;
    
    protected override void AwakeAction()
    {
        base.AwakeAction();
        audio = GetComponent<AudioSource>();
    }
    
    private void OnEnable()
    {
        if (!on)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("aedDevicePowerOn")); // 전원을 켜 제세동 에너지량(J)을 선택합니다.
    }

    [PunRPC]
    public void ContentsWorld_AedPower(bool isOn)
    {
        if (isOn)
            PowerOff();
        else
            PowerOn();
        
        audio.PlayOneShot(audio.clip);

        UpdateData();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        InfoText = on ? LocalizeManager.Instance.GetString("powerOff") : LocalizeManager.Instance.GetString("powerOn");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        InfoText = "";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        pv.RPC("ContentsWorld_AedPower", RpcTarget.All, on);
    }

    private void Update()
    {
        if (!Mathf.Approximately(value, target))
        {
            value = Mathf.MoveTowards(value, target, Time.deltaTime * 210);
            transform.localRotation = Quaternion.Euler(0, value, 0);

            if (Mathf.Approximately(value, -195))
                aed.OnWaitHandle();
        }
    }

    public void PowerOn()
    {
        on = true;
        target = -195;
        handle.GetComponent<Collider>().enabled = true;
    }

    public void PowerOff()
    {
        on = false;
        target = 15;
        handle.GetComponent<Collider>().enabled = false;
    }
    
    public override void UpdateData()
    {
        Scene.data.CartItem_Data.AED_Data.AED_Power_On = on;
        Scene.data.CartItem_Data.AED_Data.AED_Power_Target = target;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
