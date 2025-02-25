using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class AED_Charge : Interaction
{
    [SerializeField] AED aed;

    public float time;
    public bool on;
    
    private AudioSource audio;

    [PunRPC]
    public void ContentsWorld_AedCharge()
    {
        on = true;
        time = 0;
        
        aed.OnCharge();
        audio.PlayOneShot(audio.clip);
        
        UpdateData();
    }

    protected override void AwakeAction()
    {
        base.AwakeAction();
        audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (!on && aed.power.on && aed.interaction_Items.IsItem_Mount)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("chargeClick")); // Charge 버튼을 클릭하세요.
    }

    public void OnDisable()
    {
        audio.Stop();
    }

    private void Update()
    {
        if (!on)
            return;

        if (time < 1)
            time += Time.deltaTime;
        else
            CompleteCharge();
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        pv.RPC("ContentsWorld_AedCharge", RpcTarget.All);
    }

    private void CompleteCharge()
    {
        on = false;
        aed.OnWaitShock();
    }
    
    public override void UpdateData()
    {
        Scene.data.CartItem_Data.AED_Data.AED_Charge_On = on;
        Scene.data.CartItem_Data.AED_Data.AED_Charge_Time = time;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
