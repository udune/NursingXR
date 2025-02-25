using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class AED_Shock : Interaction
{
    [SerializeField] AED aed;
    private AudioSource audio;
    public bool on;

    [PunRPC]
    public void ContentsWorld_AedShock()
    {
        on = true;
        aed.OnShock();
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
        if (!on && aed.power.on && aed.charge.on && aed.interaction_Items.IsItem_Mount)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("shockClick")); // Shock 버튼을 클릭하세요.
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        pv.RPC("ContentsWorld_AedShock", RpcTarget.All);
    }
    
    public override void UpdateData()
    {
        Scene.data.CartItem_Data.AED_Data.AED_Shock_On = on;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
