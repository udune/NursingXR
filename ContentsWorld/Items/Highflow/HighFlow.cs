using System;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HighFlow : Interaction_Item
{
    protected override void AwakeAction()
    {
        base.AwakeAction();
    }

    [PunRPC]
    void ContentsWorld_DownBegin(int actorNum)
    {
        Scene.IsInteraction = true;
        step = Step.Drag;
        target.MarkerShow();
        this.actorNum = actorNum;
        holder = PhotonManager.Instance.FindCharacter(actorNum);
    }
    
    [PunRPC]
    void ContentsWorld_UpDrag()
    {
        Scene.IsInteraction = false;
        step = Step.None;
        target.MarkerHide();
    }
    
    [PunRPC]
    public void ContentsWorld_UpDetect()
    {
        Scene.IsInteraction = false;
        step = Step.Mount;
        IsItem_Mount = true;
        UpdateData_Item();
    }
    
    [PunRPC]
    void ContentsWorld_DownMount(bool isOn, int actorNum)
    {
        step = Step.DragRope;
        targetRope.MarkerShow();
        GetComponent<Collider>().enabled = false;
        this.actorNum = actorNum;
        holder = PhotonManager.Instance.FindCharacter(actorNum);
        SetRope(isOn);
    }
    
    [PunRPC]
    void ContentsWorld_UpDragRope(bool isOn)
    {
        step = Step.Mount;
        targetRope.MarkerHide();
        GetComponent<Collider>().enabled = true;
        SetRope(isOn);
    }
    
    [PunRPC]
    public void ContentsWorld_UpDetectRope(bool isOn)
    {
        step = Step.MountRope;
        IsRope_Mount = true;
        SetRope(isOn);
        
        UI_Zoom_Go.SetActive(true);
        GetComponent<Collider>().enabled = false;
        btn.GetComponent<Collider>().enabled = true;
        foreach (var outline in outlines)
            outline.enabled = false;
        
        UpdateData_Rope();
    }
    
    [Header("Item")]
    [SerializeField] Oxygen oxygen;
    [SerializeField] HighFlow_Btn btn;
    [SerializeField] public GameObject UI_Zoom_Go;
    [SerializeField] Button UI_ZoomIn_Btn;
    [SerializeField] Button UI_ZoomOut_Btn;
    private bool isVR;

    private void OnEnable()
    {
        if (!IsItem_Mount)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("deviceMoveToPole")); // 장치를 클릭하여 선반으로 옮겨주세요.
    }

    public void ZoomIn()
    {
        if (InputManager.Instance.isVR)
        {
            isVR = true; 
            camera.SwitchInterface();
        }
        
        NursingManager.Instance.character.SetMoveState(false);
        InputManager.Instance.isHighflow = true;
        UI_ZoomIn_Btn.interactable = false;
        UI_ZoomOut_Btn.interactable = true;
    }

    public void ZoomOut()
    {
        if (isVR)
        {
            camera.SwitchInterface(); 
            isVR = false;
        }
        
        NursingManager.Instance.character.SetMoveState(true);
        camera.ResetPerspectiveCamera();
        InputManager.Instance.isHighflow = false;
        UI_ZoomOut_Btn.interactable = false;
        UI_ZoomIn_Btn.interactable = true;
    }

    public override void Down_Begin()
    {
        base.Down_Begin();
        pv.RPC("ContentsWorld_DownBegin", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("deviceMoveToPole")); // 장치를 클릭하여 선반으로 옮겨주세요.
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
        if (oxygen.enabled && oxygen.IsRope_Mount)
        {
            pv.RPC("ContentsWorld_DownMount", RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("cannulaApplyToPatient")); // 장치를 클릭하여 환자에게 cannula를 적용하세요.
        }
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
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePowerOn")); // 전원을 클릭하여 장치를 켭니다.
    }
    
    public void UpdateTooltip()
    {
        
    }

    public override void UpdateData_Item()
    {
        Scene.data.CartItem_Data.HighFlow_Data.HighFlow_Item_Mount = IsItem_Mount;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }

    public override void UpdateData_Rope()
    {
        Scene.data.CartItem_Data.HighFlow_Data.HighFlow_Rope_Mount = IsRope_Mount;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}