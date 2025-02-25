using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Suction_Catheter : Interaction
{
    [SerializeField] Suction suction;
    [SerializeField] public Suction_Water water;
    
    [PunRPC]
    public void ContentsWorld_Catheter(bool power)
    {
        water.Power = !power;
        UpdateData();
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        pv.RPC("ContentsWorld_Catheter", RpcTarget.All, water.Power);
        suction.UpdateTooltip();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        InfoText = water.Power ? LocalizeManager.Instance.GetString("openValve") : LocalizeManager.Instance.GetString("closeValve");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        InfoText = "";
    }

    public override void UpdateData()
    {
        base.UpdateData();
        Scene.data.CartItem_Data.Suction_Data.Suction_Catheter_Waterpower = water.Power;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
