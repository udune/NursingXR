using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Suction_Button : Interaction
{
    [SerializeField] Suction suction;
    public int power;

    public int Power
    {
        get => power;
        set
        {
            power = value;
            UpdateTarget();
        }
    }
    
    [PunRPC]
    public void ContentsWorld_SuctionButton()
    {
        Power = ++Power % 3;
        UpdateData();
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        pv.RPC("ContentsWorld_SuctionButton", RpcTarget.All);
        suction.UpdateTooltip();
    }

    private void UpdateTarget()
    {
        switch (Power)
        {
            case 0:
                transform.DOLocalRotateQuaternion(Quaternion.Euler(90.0f, 0.0f, 0.0f), 0.5f);
                break;
            case 1:
                transform.DOLocalRotateQuaternion(Quaternion.Euler(90.0f, 0.0f, -45.0f), 0.5f);
                break;
            case 2:
                transform.DOLocalRotateQuaternion(Quaternion.Euler(90.0f, 0.0f, 45.0f), 0.5f);
                break;
        }
    }
    
    public override void UpdateData()
    {
        base.UpdateData();
        Scene.data.CartItem_Data.Suction_Data.Suction_Btn_Power = Power;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
