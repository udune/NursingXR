using System;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spo2 : Interaction_Item
{
    [PunRPC]
    void ContentsWorld_DownBegin()
    {
        step = Step.Drag;
        target.MarkerShow();
    }
    
    [PunRPC]
    void ContentsWorld_UpDrag()
    {
        step = Step.None;
        target.MarkerHide();
    }
    
    [PunRPC]
    void ContentsWorld_UpDetect()
    {
        step = Step.Mount;
        IsItem_Mount = true;
        UpdateData_Item();
    }
    
    [PunRPC]
    void ContentsWorld_DownMount(bool isOn, int actorNum)
    {
        Scene.IsInteraction = true;
        step = Step.DragRope;
        targetRope.MarkerShow();
        this.actorNum = actorNum;
        holder = PhotonManager.Instance.FindCharacter(actorNum);
        SetRope(isOn);
        if (headNode != null) headNode.gameObject.SetActive(true);
    }
    
    [PunRPC]
    void ContentsWorld_UpDragRope(bool isOn)
    {
        Scene.IsInteraction = false;
        step = Step.Mount;
        targetRope.MarkerHide();
        SetRope(isOn);
        if (headNode != null) headNode.gameObject.SetActive(false);
    }
    
    [PunRPC]
    public void ContentsWorld_UpDetectRope(bool isOn)
    {
        Scene.IsInteraction = false;
        step = Step.PowerOn;
        IsRope_Mount = true;
        SetRope(isOn);
        
        zoomUI.SetActive(true);
        screen_Off.SetActive(false);
        model.transform.localEulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
        
        UpdateData_Rope();
        if (headNode != null) headNode.gameObject.SetActive(false);
    }
    
    [PunRPC]
    public void ContentsWorld_SpoButton(bool on)
    {
        screen_On.SetActive(on);
        this.on = !on;
        UpdateData();
    }

    [Header("Item")]
    [SerializeField] public GameObject screen_On;
    [SerializeField] public GameObject screen_Off;
    [SerializeField] public GameObject screen_98;
    [SerializeField] public GameObject model;
    [SerializeField] public GameObject zoomUI;
    public bool on;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (IsRope_Mount)
            InfoText = on ? LocalizeManager.Instance.GetString("powerOff") : LocalizeManager.Instance.GetString("powerOn");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (IsRope_Mount)
            InfoText = "";
    }
    
    private void OnEnable()
    {
        if (IsRope_Mount)
        {
            targetRope.Mount();
            zoomUI.SetActive(true);
            if (!on)
                contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePressOn")); // 장치를 눌러 작동시키세요.
        }
        else
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("bodyToPickupSensor")); // 본체를 눌러 센서를 집으세요.
    }

    private void OnDisable()
    {
        targetRope.Remove();
        
        zoomUI.SetActive(false);
        if (NursingManager.Instance.character != null) NursingManager.Instance.character.SetMoveState(true);
        CameraManager camera = FindObjectOfType<CameraManager>();
        if (camera != null) camera.ResetPerspectiveCamera();
        InputManager.Instance.isSpo = false;
        if (Scene != null)
        {
            Scene.cart.UI_ZoomOut_SPO_Btn.interactable = false;
            Scene.cart.UI_ZoomIn_SPO_Btn.interactable = true;
        }
    }
    
    public override void Down_Begin()
    {
        base.Down_Begin();
        pv.RPC("ContentsWorld_DownBegin", RpcTarget.All);
    }

    protected override void Update_Drag()
    {
        base.Update_Drag();
        step = CheckTarget() ? Step.Detect : Step.Drag;
    }

    protected override void Up_Drag()
    {
        base.Up_Drag();
        pv.RPC("ContentsWorld_UpDrag", RpcTarget.All);
    }

    protected override void Up_Detect()
    {
        base.Up_Detect();
        pv.RPC("ContentsWorld_UpDetect", RpcTarget.All);
    }
    
    protected override void Down_Mount()
    {
        base.Down_Mount();
        pv.RPC("ContentsWorld_DownMount", RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("sensorToMarkedArea")); // 센서를 표시된 부분에 가져가세요.
    }

    protected override void Update_Rope()
    {
        base.Update_Rope();
        step = CheckTargetRope() ? Step.DetectRope : Step.DragRope;
    }
    
    protected override void Up_DragRope()
    {
        base.Up_DragRope();
        pv.RPC("ContentsWorld_UpDragRope", RpcTarget.All, false);
    }

    protected override void Up_DetectRope()
    {
        base.Up_DetectRope();
        pv.RPC("ContentsWorld_UpDetectRope", RpcTarget.All, false);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePressOn")); // 장치를 눌러 작동시키세요.
    }

    protected override void Down_PowerOn()
    {
        base.Down_PowerOn();
        pv.RPC("ContentsWorld_SpoButton", RpcTarget.All, on);
    }
    
    public void UpdateTooltip()
    {
        
    }

    public override void UpdateData_Item()
    {
        
    }

    public override void UpdateData()
    {
        base.UpdateData();
        Scene.data.CartItem_Data.SPO_Data.SPO_On = on;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }

    public override void UpdateData_Rope()
    {
        Scene.data.CartItem_Data.SPO_Data.SPO_Rope_Mount = IsRope_Mount;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
