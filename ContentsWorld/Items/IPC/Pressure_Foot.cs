using System;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pressure_Foot : Interaction_Item
{
    public enum FootType
    {
        Left,
        Right
    }
    
    [SerializeField] private FootType type;
    
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
    void ContentsWorld_UpDetect()
    {
        Scene.IsInteraction = false;
        step = Step.Mount;
        IsItem_Mount = true;
        scale = transform.localScale;
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
    
    [SerializeField] Pressure pressure;

    private Vector3 scale = Vector3.one;
    public float Scale
    {
        set
        {
            if ((value *= 2) < 1)
                value = 1 - 0.05f * (Mathf.Sqrt(1 - value * value) - 1);
            else
                value = 1 + 0.05f * (Mathf.Sqrt(1 - (value -= 2) * value) + 1);

            transform.localScale = new Vector3(scale.x * value, scale.y * value, scale.z);
        }
    }

    private void OnEnable()
    {
        if (!IsItem_Mount)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("cuffMoveToLeg")); // 커프를 환자 다리로 옮기세요.
    }

    public override void Down_Begin()
    {
        base.Down_Begin();
        pv.RPC("ContentsWorld_DownBegin", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("cuffMoveToLeg")); // 커프를 환자 다리로 옮기세요.
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
    
    public override void UpdateData_Item()
    {
        switch (type)
        {
            case FootType.Left:
                Scene.data.CartItem_Data.Pressure_Data.Pressure_Left_Foot_Item_Mount = IsItem_Mount;
                break;
            case FootType.Right:
                Scene.data.CartItem_Data.Pressure_Data.Pressure_Right_Foot_Item_Mount = IsItem_Mount; 
                break;
        }
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
