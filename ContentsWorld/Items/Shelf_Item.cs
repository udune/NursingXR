using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shelf_Item : Interaction
{
    enum Type { Neb = 0, Is, AED, Oxygen, IPC, HighFlow, SPO, Suction }

    [Header("Type")] 
    [SerializeField] private Type type; 
    
    [Header("CartList")]
    [SerializeField] GameObject cartIcon;
    
    [Header("GameObject")]
    [SerializeField] GameObject collectingGo;
    [SerializeField] GameObject collectedGo;

    [Header("Info")] 
    [SerializeField] GameObject info;

    private bool isCollected;

    [PunRPC]
    public void ContentsWorld_ShelfItem(bool isCollected)
    {
        if (isCollected)
        {
            IsCollected();
            UpdateData(false);
        }
        else
        {
            Collected();
            UpdateData(true);
        }
    }

    // 카트에서 삭제
    private void IsCollected()
    {
        isCollected = false;
        
        contentsWorldUI.cartInventoryUI.OnCheckIndex((int) type);
        contentsWorldUI.cartInventoryUI.UpdateCartList();
        
        cartIcon.SetActive(false);
        
        SetGo(true, false);
    }

    // 카트에 담기
    private void Collected()
    {
        isCollected = true;
        
        contentsWorldUI.cartInventoryUI.UpdateCartList();
        
        cartIcon.SetActive(true);
        
        SetGo(false, true);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        if (Scene.IsInteraction) return;
        pv.RPC("ContentsWorld_ShelfItem", RpcTarget.All, isCollected);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        InfoText = isCollected ? LocalizeManager.Instance.GetString("removeCart") : LocalizeManager.Instance.GetString("addCart"); // 카트에서 삭제
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        InfoText = "";
    }

    // 물품 설명을 띄웁니다.
    public void OnInfo()
    {
        bool isOn = info.activeSelf;
        info.SetActive(!isOn);
    }

    private void SetGo(bool collecting, bool collected)
    {
        collectingGo.SetActive(collecting);
        collectedGo.SetActive(collected);
    }

    // 불러온 포톤 데이터를 물품에 적용합니다.
    public override void LoadWorld()
    {
        switch (type)
        {
            case Type.Neb:
                UpdateEvent(Scene.data.ShelfItem_Data.Neb);
                break;
            case Type.Is:
                UpdateEvent(Scene.data.ShelfItem_Data.IS);
                break;
            case Type.AED:
                UpdateEvent(Scene.data.ShelfItem_Data.AED);
                break;
            case Type.Oxygen:
                UpdateEvent(Scene.data.ShelfItem_Data.Oxygen);
                break;
            case Type.IPC:
                UpdateEvent(Scene.data.ShelfItem_Data.IPC);
                break;
            case Type.HighFlow:
                UpdateEvent(Scene.data.ShelfItem_Data.HighFlow);
                break;
            case Type.SPO:
                UpdateEvent(Scene.data.ShelfItem_Data.SPO);
                break;
            case Type.Suction:
                UpdateEvent(Scene.data.ShelfItem_Data.Suction);
                break;
        }
    }
    
    // 불러온 포톤 데이터를 물품에 적용합니다.
    private void UpdateEvent(bool active)
    {
        if (active)
            Collected();
        else
            IsCollected();
    }

    public override void UpdateData(bool active)
    {
        switch (type)
        {
            case Type.Neb:
                Scene.data.ShelfItem_Data.Neb = active;
                break;
            case Type.Is:
                Scene.data.ShelfItem_Data.IS = active;
                break;
            case Type.AED:
                Scene.data.ShelfItem_Data.AED = active;
                break;
            case Type.Oxygen:
                Scene.data.ShelfItem_Data.Oxygen = active;
                break;
            case Type.IPC:
                Scene.data.ShelfItem_Data.IPC = active;
                break;
            case Type.HighFlow:
                Scene.data.ShelfItem_Data.HighFlow = active;
                break;
            case Type.SPO:
                Scene.data.ShelfItem_Data.SPO = active;
                break;
            case Type.Suction:
                Scene.data.ShelfItem_Data.Suction = active;
                break;
        }

        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
