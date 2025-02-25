using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Nebulizer_Btn : Interaction
{
    [SerializeField] Nebulizer_Handle nebulizer_Handle;
    public GameObject particle;

    public bool Power;

    private AudioSource audio;
    
    [PunRPC]
    public void ContentsWorld_NebButton(bool power)
    {
        if (power)
            PowerOff();
        else
            PowerOn();

        audio.PlayOneShot(audio.clip);
        UpdateData();
    }

    protected override void AwakeAction()
    {
        base.AwakeAction();
        audio = GetComponent<AudioSource>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        InfoText = Power
            ? LocalizeManager.Instance.GetString("powerOff")
            : LocalizeManager.Instance.GetString("powerOn");
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
        pv.RPC("ContentsWorld_NebButton", RpcTarget.All, Power);
    }

    private void PowerOn()
    {
        Power = true;
        particle.SetActive(true);
        contentsWorldUI.toolTip.SetTooltip("");
    }

    public void PowerOff()
    {
        Power = false;
        particle.SetActive(false);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePressOn")); // 전원 버튼을 눌러 전원을 켜주세요.
    }
    
    public override void UpdateData()
    {
        Scene.data.CartItem_Data.Nebulizer_Data.Nebulizer_Btn_Power = Power;
        Scene.data.CartItem_Data.Nebulizer_Data.Nebulizer_Btn_Enable = GetComponent<Collider>().enabled;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
    
}
