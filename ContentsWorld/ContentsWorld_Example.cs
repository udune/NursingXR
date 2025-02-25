using Constants;
using Photon.Pun;
using UnityEngine;

public class ContentsWorld_Example : Interaction_Item
{
    [PunRPC]
    void ContentsWorld_DownBegin()
    {
        Scene.IsInteraction = true;
        step = Step.Drag;
        target.MarkerShow();
    }
    
    [PunRPC]
    void ContentsWorld_UpDrag()
    {
        Scene.IsInteraction = false;
        step = Step.None;
        target.MarkerHide();
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }
    
    [PunRPC]
    void ContentsWorld_UpDetect()
    {
        Scene.IsInteraction = false;
        step = Step.Mount;
        IsItem_Mount = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        UpdateData();
    }
    
    [PunRPC]
    void ContentsWorld_DownMount(bool isOn, int actorNum)
    {
        step = Step.DragRope;
        targetRope.MarkerShow();
        holder = PhotonManager.Instance.FindCharacter(actorNum);
        SetRope(isOn);
    }
    
    [PunRPC]
    void ContentsWorld_UpDragRope(bool isOn)
    {
        step = Step.Mount;
        targetRope.MarkerHide();
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
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
        
        UpdateData();
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
        contentsWorldUI.toolTip.SetTooltip("");
    }
    
    public void UpdateTooltip()
    {
        
    }
    
    public override void UpdateData()
    {
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
