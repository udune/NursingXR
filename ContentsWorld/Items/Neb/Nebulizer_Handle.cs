using System;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Nebulizer_Handle : Interaction_Item
{
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
        btn.GetComponent<Collider>().enabled = true;
        UpdateData_Item();
    }
    
    [PunRPC]
    void ContentsWorld_DownMount(bool isOn, int actorNum)
    {
        step = Step.DragRope;
        targetRope.MarkerShow();
        this.actorNum = actorNum;
        holder = PhotonManager.Instance.FindCharacter(actorNum);
        SetRope(isOn);
    }
    
    [PunRPC]
    void ContentsWorld_UpDragRope(bool isOn)
    {
        step = Step.Mount;
        targetRope.MarkerHide();
        SetRope(isOn);
    }
    
    [PunRPC]
    void ContentsWorld_UpDetectRope(bool isOn)
    {
        step = Step.MountRope;
        IsRope_Mount = true;
        SetRope(isOn);
        
        GetComponent<Collider>().enabled = false;
        foreach (var outline in outlines)
            outline.enabled = false;
        
        UpdateData_Rope();
    }

    private void OnEnable()
    {
        if (!IsItem_Mount)
        {
            btn.PowerOff();
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("maskApplyToPatient")); // 마스크를 환자에게 적용시키세요.
        }
    }

    public override void Down_Begin()
    {
        base.Down_Begin();
        pv.RPC("ContentsWorld_DownBegin", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("maskApplyToPatient")); // 마스크를 환자에게 적용시키세요.
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
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePressOn")); // 전원 버튼을 눌러 전원을 켜주세요.
    }
    
    protected override void Down_Mount()
    {
        base.Down_Mount();
        pv.RPC("ContentsWorld_DownMount", RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
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
    }
    
    [SerializeField] public Nebulizer_Btn btn;
    private Interaction_Item interaction_Item;
    private Outline outline;
    private BoxCollider collider;
    
    public void UpdateTooltip()
    {
        
    }

    public override void UpdateData_Item()
    {
        Scene.data.CartItem_Data.Nebulizer_Data.Nebulizer_Item_Mount = IsItem_Mount;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
