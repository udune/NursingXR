using System;
using System.Collections;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interaction_Item : Interaction, IPunObservable
{
    public Step step = Step.None;

    // 물품이 장착되었는지 여부
    public bool isItem_Mount;
    public bool IsItem_Mount
    {
        get => isItem_Mount;
        set
        {
            isItem_Mount = value;
            if (value)
            {
                target?.Mount();
                gameObject.SetActive(false);
            }
        }
    }
    
    // 물품 케이블이 연결되었는지 여부
    public bool isRope_Mount;
    public bool IsRope_Mount
    {
        get => isRope_Mount;
        set
        {
            isRope_Mount = value;
            if (value)
                targetRope?.Mount();
        }
    }
    
    [Header("Target")]
    public Target target;
    public Target targetRope;
    
    [Header("Rope")]
    public Rope rope;
    [SerializeField] protected Transform startNode;
    [SerializeField] protected Transform endNode;
    [SerializeField] protected Transform headNode;
    
    private Vector3 syncPos;
    private Quaternion syncRot;
    protected int actorNum;
    protected GameObject holder;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            SendMessage(stream);
            return;
        }

        ReceiveMessage(stream);
    }
    
    protected override void StartAction()
    {
        syncPos = transform.localPosition;
        syncRot = transform.localRotation;
    }
    
    private void SendMessage(PhotonStream stream)
    {
        stream.SendNext(transform.localPosition);
        stream.SendNext(transform.localRotation);
    }

    private void ReceiveMessage(PhotonStream stream)
    {
        syncPos = (Vector3) stream.ReceiveNext();
        syncRot = (Quaternion) stream.ReceiveNext();
    }

    private void FixedUpdate()
    {
        if (Scene.character.isObserver) return;
        
        // 물품 움직임을 실시간 동기화합니다.
        transform.localPosition = Vector3.Lerp(transform.localPosition, syncPos, Time.deltaTime * 20.0f);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, syncRot, Time.deltaTime * 20.0f);
    }

    protected virtual void Update()
    {
        if (Scene.character.isObserver) return;
        
        switch (step)
        {
            case Step.None:
                break;
            case Step.Drag:
            case Step.Detect:
                Update_Drag();
                break;
            case Step.Mount:
                break;
            case Step.DragRope:
            case Step.DetectRope:
                Update_Rope();
                break;
            case Step.MountRope:
                break;
            case Step.PowerOn:
                break;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Scene.character.isObserver || Scene.IsInteraction) return;
        switch (step)
        {
            case Step.None:
                Down_Begin();
                break;
            case Step.Drag:
                break;
            case Step.Detect:
                break;
            case Step.Mount:
                Down_Mount();
                break;
            case Step.DragRope:
                break;
            case Step.DetectRope:
                break;
            case Step.MountRope:
                break;
            case Step.PowerOn:
                Down_PowerOn();
                break;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Scene.character.isObserver) return;
        switch (step)
        {
            case Step.None:
                break;
            case Step.Drag:
                Up_Drag();
                break;
            case Step.Detect:
                Up_Detect();
                break;
            case Step.Mount:
                break;
            case Step.DragRope:
                Up_DragRope();
                break;
            case Step.DetectRope:
                Up_DetectRope();
                break;
            case Step.MountRope:
                break;
            case Step.PowerOn:
                break;
        }
    }

    // 물품을 처음 클릭했을때
    public virtual void Down_Begin()
    {
        Debug.Log("Begin");
    }
    
    // 물품을 들고 있을때
    protected virtual void Update_Drag()
    {
        Debug.Log("Update Drag");
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNum)
        {
            // 나일 경우에는 물품이 마우스 움직임을 따라갑니다.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.origin + ray.direction * 1.5f;
        }
        else
        {
            // 다른 유저일 경우에는 캐릭터의 손 포지션을 따라갑니다.
            Transform handTrn = holder.GetComponent<CharacterManager>().characterAnimator.GetComponent<CharacterBody>().hand.transform;
            transform.position = handTrn.position;
        }
    }

    // 타겟을 찾지 못한 상태에서 물품을 놓았을때
    protected virtual void Up_Drag()
    {
        Debug.Log("Up Drag");
    }

    // 타겟을 찾은 상태에서 물품을 놓았을때
    protected virtual void Up_Detect()
    {
        Debug.Log("Up Detect");
    }

    // 장착된 물품을 클릭했을때
    protected virtual void Down_Mount()
    {
        Debug.Log("Down Mount");
    }

    // 물품에 연결된 케이블을 들고 있을때
    protected virtual void Update_Rope()
    {
        Debug.Log("Update Rope");
        if (startNode != null && endNode != null)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == actorNum)
            {
                // 나일 경우에는 케이블이 마우스 움직임을 따라갑니다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                startNode.position = ray.origin + ray.direction * 1.5f;
                endNode.position = NursingManager.Instance.character.transform.position;
            }
            else
            {
                // 다른 유저일 경우에는 케이블이 손의 포지션을 따라갑니다.
                Transform handTrn = holder.GetComponent<CharacterManager>().characterAnimator.GetComponent<CharacterBody>().hand.transform;
                startNode.position = handTrn.position + (holder.transform.up * 0.5f) + (holder.transform.forward * 0.5f);
                endNode.position = handTrn.position;
            }
        }
    }

    // 타겟을 찾지 못한 상태에서 케이블을 놓았을 때
    protected virtual void Up_DragRope()
    {
        Debug.Log("Up DragRope");
    }

    // 타겟을 찾은 상태에서 케이블을 놓았을 때
    protected virtual void Up_DetectRope()
    {
        Debug.Log("Up DetectRope");
    }

    // 물품에 전원을 켰을때
    protected virtual void Down_PowerOn()
    {
        Debug.Log("Down PowerOn");
    }
    
    // 케이블 초기 세팅을 합니다.
    protected virtual void SetRope(bool isOn)
    {
        rope.ResetRopePosition();
        rope.SetActive_Renderer(holder, isOn);
    }
    
    // 물품을 장착시킬 위치 타겟을 찾습니다.
    protected bool CheckTarget()
    {
        if (target != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, hit, 10, 1 << 3) > 0)
            {
                if (hit.Length > 0)
                {
                    foreach (var obj in hit)
                    {
                        Debug.Log($"Detect {obj.transform.gameObject.name} {target.gameObject.name}");
                        if (obj.transform == target.targetTrn)
                            return true;
                        return false;
                    }
                }
            }
            Debug.DrawRay(ray.origin,ray.direction * 10.0f);
        }
        return false;
    }

    // 물품 케이블을 연결시킬 타겟을 찾습니다.
    protected bool CheckTargetRope()
    {
        if (targetRope != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hit = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, hit, 10, 1 << 3) > 0)
            {
                if (hit.Length > 0)
                {
                    foreach (var obj in hit)
                    {
                        Debug.Log($"Detect {obj.transform.gameObject.name} {targetRope.gameObject.name}");
                        if (obj.transform == targetRope.targetTrn)
                            return true;
                        return false;
                    }
                }
            }
            Debug.DrawRay(ray.origin,ray.direction * 10.0f);
        }
        return false;
    }
}