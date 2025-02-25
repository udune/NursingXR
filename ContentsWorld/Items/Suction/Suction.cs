using System;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Suction : Interaction_Item
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
        water.ResetHeight();
        
        UI_Zoom_Go.SetActive(true);
        GetComponent<Collider>().enabled = false;
        suctionBtn.GetComponent<Collider>().enabled = true;
        foreach (var outline in outlines)
            outline.enabled = false;
        
        UpdateData_Rope();
    }
    
    [Header("Item")]
    [SerializeField] Suction_Button suctionBtn;
    [SerializeField] public Suction_Water water;
    [SerializeField] public GameObject UI_Zoom_Go;
    [SerializeField] Button UI_ZoomIn_Btn;
    [SerializeField] Button UI_ZoomOut_Btn;
    private bool isVR;

    private void OnEnable()
    {
        if (!IsItem_Mount)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("suctionMoveToPosition")); // 석션을 눌러 표시된 위치로 옮겨주세요.
    }
    
    public void ZoomIn()
    {
        if (InputManager.Instance.isVR)
        {
            isVR = true; 
            camera.SwitchInterface();
        }

        NursingManager.Instance.character.SetMoveState(false);
        InputManager.Instance.isSuction = true;
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
        InputManager.Instance.isSuction = false;
        UI_ZoomOut_Btn.interactable = false;
        UI_ZoomIn_Btn.interactable = true;
    }

    public override void Down_Begin()
    {
        base.Down_Begin();
        pv.RPC("ContentsWorld_DownBegin", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("suctionMoveToPosition")); // 석션을 눌러 표시된 위치로 옮겨주세요.
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
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("suctionCatheterBringToPatient")); // 석션을 눌러 환자 입에 카테터를 가져가세요.
    }
    
    protected override void Down_Mount()
    {
        base.Down_Mount();
        pv.RPC("ContentsWorld_DownMount", RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("suctionCatheterBringToPatient")); // 석션을 눌러 환자 입에 카테터를 가져가세요.
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
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("blockCatheter")); // 카테터를 막아주세요.
    }

    public void UpdateTooltip()
    {
        if (suctionBtn.Power == 0)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("adjustPowerGauge")); // 게이지 버튼을 통해 파워를 조절해주세요.
    }
    
    public override void UpdateData_Item()
    {
        Scene.data.CartItem_Data.Suction_Data.Suction_Item_Mount = IsItem_Mount;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
    
    public override void UpdateData_Rope()
    {
        Scene.data.CartItem_Data.Suction_Data.Suction_Rope_Mount = IsRope_Mount;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
