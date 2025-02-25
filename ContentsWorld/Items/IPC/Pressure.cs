using System;
using System.Collections.Generic;
using Constants;
using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Pressure : Interaction_Item
{
    public Dictionary<int, bool> ropeDic = new () { {0, false}, {1, false}, {2, false}, {3, false}, {4, false}, {5, false}};
    public List<Target> targetRopeList = new List<Target>();
    public List<Rope> ropeList = new List<Rope>();
    [SerializeField] public Pressure_Foot pressure_Foot_Left;
    [SerializeField] public Pressure_Foot pressure_Foot_Right;

    private AudioSource audio;
    
    public bool on;
    private float value;
    public float _target;

    protected override void AwakeAction()
    {
        base.AwakeAction();
        audio = GetComponent<AudioSource>();
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
    void ContentsWorld_UpDetect()
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
        
        if (!ropeDic[0]) 
            targetRopeList[0].MarkerShow();
        else if (!ropeDic[1])
            targetRopeList[1].MarkerShow();
        else if (!ropeDic[2])
            targetRopeList[2].MarkerShow();
        else if (!ropeDic[3])
            targetRopeList[3].MarkerShow();
        else if (!ropeDic[4])
            targetRopeList[4].MarkerShow();
        else if (!ropeDic[5])
            targetRopeList[5].MarkerShow();

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
        
        if (!ropeDic[0]) 
            targetRopeList[0].MarkerHide();
        else if (!ropeDic[1])
            targetRopeList[1].MarkerHide();
        else if (!ropeDic[2])
            targetRopeList[2].MarkerHide();
        else if (!ropeDic[3])
            targetRopeList[3].MarkerHide();
        else if (!ropeDic[4])
            targetRopeList[4].MarkerHide();
        else if (!ropeDic[5])
            targetRopeList[5].MarkerHide();
        GetComponent<Collider>().enabled = true;
        
        SetRope(isOn);
    }
    
    [PunRPC]
    void ContentsWorld_UpDetectRope(bool isOn)
    {
        step = Step.Mount;

        SetRope(isOn);
        
        targetRope.Mount();
        
        if (!ropeDic[0])
            ropeDic[0] = true;
        else if (!ropeDic[1])
            ropeDic[1] = true;
        else if (!ropeDic[2])
            ropeDic[2] = true;
        else if (!ropeDic[3])
            ropeDic[3] = true;
        else if (!ropeDic[4])
            ropeDic[4] = true;
        else if (!ropeDic[5])
        {
            ropeDic[5] = true;
            step = Step.PowerOn;
        }
        GetComponent<Collider>().enabled = true;
        
        UpdateData_Rope();
    }
    
    [PunRPC]
    public void ContentsWorld_PressurePower(bool isOn)
    {
        if (isOn)
            PowerOff();
        else
            PowerOn();
        
        audio.PlayOneShot(audio.clip);
        
        UpdateData();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        if (IsItem_Mount && ropeDic[0] && ropeDic[1] && ropeDic[2] && ropeDic[3] && ropeDic[4] && ropeDic[5])
            InfoText = on ? LocalizeManager.Instance.GetString("powerOff") : LocalizeManager.Instance.GetString("powerOn");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (IsItem_Mount && ropeDic[0] && ropeDic[1] && ropeDic[2] && ropeDic[3] && ropeDic[4] && ropeDic[5])
            InfoText = "";
    }

    private void PowerOn()
    {
        on = true;
        _target = 0;
    }

    private void PowerOff()
    {
        on = false;
    }

    protected override void SetRope(bool isOn)
    {
        if (!ropeDic[0])
        {
            targetRope = targetRopeList[0];
            ropeList[0].ResetRopePosition();
            ropeList[0].SetActive_Renderer(holder, isOn);
        }
        else if (!ropeDic[1])
        {
            targetRope = targetRopeList[1];
            ropeList[1].ResetRopePosition();
            ropeList[1].SetActive_Renderer(holder, isOn);
        }
        else if (!ropeDic[2])
        {
            targetRope = targetRopeList[2];
            ropeList[2].ResetRopePosition();
            ropeList[2].SetActive_Renderer(holder, isOn);
        }
        else if (!ropeDic[3])
        {
            targetRope = targetRopeList[3];
            ropeList[3].ResetRopePosition();
            ropeList[3].SetActive_Renderer(holder, isOn);
        }
        else if (!ropeDic[4])
        {
            targetRope = targetRopeList[4];
            ropeList[4].ResetRopePosition();
            ropeList[4].SetActive_Renderer(holder, isOn);
        }
        else if (!ropeDic[5])
        {
            targetRope = targetRopeList[5];
            ropeList[5].ResetRopePosition();
            ropeList[5].SetActive_Renderer(holder, isOn);
        }
    }

    private void OnEnable()
    {
        if (!IsItem_Mount)
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("deviceMoveToBed")); // 장치를 침상으로 옮기세요.
    }

    public override void Down_Begin()
    {
        base.Down_Begin();
        pv.RPC("ContentsWorld_DownBegin", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("deviceMoveToBed")); // 장치를 침상으로 옮기세요.
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
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("deviceConnectToCuff")); // 장치를 눌러 커프에 연결하세요.
    }
    
    protected override void Down_Mount()
    { 
        base.Down_Mount();
        
        if (pressure_Foot_Right.transform.parent.gameObject.activeSelf && pressure_Foot_Left.transform.parent.gameObject.activeSelf)
        {
            pv.RPC("ContentsWorld_DownMount", RpcTarget.All, true, PhotonNetwork.LocalPlayer.ActorNumber);
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("deviceConnectToCuff")); // 장치를 눌러 커프에 연결하세요.
            return;
        }
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("cuffMoveToLeg")); // 커프를 환자 다리로 옮기세요.
    }

    protected override void Update_Rope()
    {
        base.Update_Rope();
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNum)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!ropeDic[0])
            {
                ropeList[0].startNode.position = ray.origin + ray.direction * 1.5f;
                ropeList[0].endNode.position = NursingManager.Instance.character.transform.position;
            }
            else if (!ropeDic[1])
            {
                ropeList[1].startNode.position = ray.origin + ray.direction * 1.5f;
                ropeList[1].endNode.position = NursingManager.Instance.character.transform.position;
            }
            else if (!ropeDic[2])
            {
                ropeList[2].startNode.position = ray.origin + ray.direction * 1.5f;
                ropeList[2].endNode.position = NursingManager.Instance.character.transform.position;
            }
            else if (!ropeDic[3])
            {
                ropeList[3].startNode.position = ray.origin + ray.direction * 1.5f;
                ropeList[3].endNode.position = NursingManager.Instance.character.transform.position;
            }
            else if (!ropeDic[4])
            {
                ropeList[4].startNode.position = ray.origin + ray.direction * 1.5f;
                ropeList[4].endNode.position = NursingManager.Instance.character.transform.position;
            }
            else if (!ropeDic[5])
            {
                ropeList[5].startNode.position = ray.origin + ray.direction * 1.5f;
                ropeList[5].endNode.position = NursingManager.Instance.character.transform.position;
            }
        }
        else
        {
            Transform handTrn = holder.GetComponent<CharacterManager>().characterAnimator.GetComponent<CharacterBody>().hand.transform;
            
            if (!ropeDic[0])
            {
                ropeList[0].startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                ropeList[0].endNode.position = handTrn.position;
            }
            else if (!ropeDic[1])
            {
                ropeList[1].startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                ropeList[1].endNode.position = handTrn.position;
            }
            else if (!ropeDic[2])
            {
                ropeList[2].startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                ropeList[2].endNode.position = handTrn.position;
            }
            else if (!ropeDic[3])
            {
                ropeList[3].startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                ropeList[3].endNode.position = handTrn.position;
            }
            else if (!ropeDic[4])
            {
                ropeList[4].startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                ropeList[4].endNode.position = handTrn.position;
            }
            else if (!ropeDic[5])
            {
                ropeList[5].startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                ropeList[5].endNode.position = handTrn.position;
            }
        }
        
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
        if (ropeDic[0] && ropeDic[1] && ropeDic[2] && ropeDic[3] && ropeDic[4] && !ropeDic[5])
            contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePressOn")); // 장치를 눌러 작동시키세요.
        pv.RPC("ContentsWorld_UpDetectRope", RpcTarget.All, false);
    }

    protected override void Down_PowerOn()
    {
        base.Down_PowerOn();
        contentsWorldUI.toolTip.SetTooltip(on ? LocalizeManager.Instance.GetString("devicePressOn") : ""); // 장치를 눌러 작동시키세요.
        pv.RPC("ContentsWorld_PressurePower", RpcTarget.All, on);
    }
    
    public void UpdateTooltip()
    {
        
    }

    public override void UpdateData_Item()
    {
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Item_Mount = IsItem_Mount;   
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
    
    public override void UpdateData_Rope()
    {
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Rope_Mount_1 = ropeDic[0];
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Rope_Mount_2 = ropeDic[1];
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Rope_Mount_3 = ropeDic[2];
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Rope_Mount_4 = ropeDic[3];
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Rope_Mount_5 = ropeDic[4];
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Rope_Mount_6 = ropeDic[5];
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }

    public override void UpdateData()
    {
        Scene.data.CartItem_Data.Pressure_Data.Pressure_On = on;
        Scene.data.CartItem_Data.Pressure_Data.Pressure_Target = _target;
    }

    protected override void Update()
    {
        base.Update();
        
        if (!Mathf.Approximately(value, _target))
        {
            value = Mathf.MoveTowards(value, _target, Time.deltaTime * 0.5f);
            pressure_Foot_Left.Scale = value;
            pressure_Foot_Right.Scale = value;
        }
        else
        {
            if (on)
                _target = _target == 0 ? 1 : 0;
        }
    }
}
