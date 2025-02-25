using System.Collections;
using UnityEngine;
using Photon.Pun;
using Constants;
using System;
public enum WheelChairOption { footUP, footDown, Lock, UnLock, Hold, Drop, SitOn, SitOff };
public class WheelChair : MonoBehaviour, IPunObservable
{
    [SerializeField] ContentsWorldScene Scene;
    [SerializeField] ContentsWorldUI contentsWorldUI;
    [SerializeField] public PhotonView pv;
    [SerializeField] public Animator animator_Foot;
    [SerializeField] Animator animator_Lock;
    [SerializeField] Animator animator_LockUI;
    [SerializeField] Transform sitOffPos; // 앉은 후 일어나는 자리, 휠체어 잡는 자리
    [SerializeField] public GameObject ui;

    WheelChairOption wheelChairOption;
    // 처음 입장시 동기화
    public GameObject holder;
    public bool isHolding = false;
    public bool isUp = false;
    public bool isLock = true;
    public bool isSit = false; // rpc
    public bool isWallCheck;
    public bool isObserver => Scene.character.isObserver;

    public string InfoText
    {
        set
        {
            if (string.IsNullOrEmpty(value))
                contentsWorldUI.infoPanel.SetDisable();
            else
                contentsWorldUI.infoPanel.SetEnable(value);
        }
    }

    private void Update()
    {
        if (isHolding)
        {
            if (Input.GetMouseButton(0))
                MoveWheelChair();
        }
    }

    void InitWheelChairAnim()
    {
        OnBoolAnimFoot(AnimParam.ANIM_FOOTUP, isUp);
        OnBoolAnimLock(AnimParam.ANIM_LOCK, isLock);
        OnBoolAnimLockUI(AnimParam.ANIM_LOCKUI, isLock);
    }

    public void ChangeWheelChairOption(WheelChairOption _wheelChairOption)
    {
        wheelChairOption = _wheelChairOption;
        switch (wheelChairOption)
        {
            case WheelChairOption.Lock:
                OpenWheelCahirLock(AnimParam.ANIM_LOCK, true);
                OpenWheelChairLockUI(AnimParam.ANIM_LOCKUI, true);
                break;
            case WheelChairOption.UnLock:
                OpenWheelCahirLock(AnimParam.ANIM_LOCK, false);
                OpenWheelChairLockUI(AnimParam.ANIM_LOCKUI, false);
                break;
            case WheelChairOption.SitOn:
                SitOnWheelChair();
                UpWheelChairFoot(AnimParam.ANIM_FOOTUP, false);
                break;
            case WheelChairOption.SitOff:
                SitOffWheelChair();
                break;
        }
    }
    
    public void WheelchairPushEvent(bool isTrue)
    {
        pv.RPC("ContentsWorld_WheelChairPush", RpcTarget.All, isTrue, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    [PunRPC]
    public void SetParent(int userId)
    {
        GameObject sitPlayer = PhotonManager.Instance.FindCharacter_UserId(userId);
        if (sitPlayer != null)
            sitPlayer.transform.SetParent(transform);

        UpdateData(true, userId);
    }
    
    [PunRPC]
    public void ContentsWorld_WheelChairPush(bool isTrue, int actorNum)
    {
        // 휠체어를 잡은 플레이어 오브젝트를 찾습니다.
        holder = PhotonManager.Instance.FindCharacter(actorNum);
        isHolding = isTrue;
        if (!isHolding)
            RespawnWheelchair();
    }

    public void InfoMessage(string _msg)
    {
        InfoText = _msg;
    }
    
    private void MoveWheelChair()
    {
        Vector3 position = holder.transform.position;
        Quaternion rotation = holder.transform.rotation;

        Vector3 forward = rotation * Vector3.forward;
        Vector3 position_forward = position + forward;
        position_forward.y = -2.0f;

        transform.position = position_forward;
        transform.LookAt(position_forward + forward, Vector3.up);
        Quaternion rot = transform.rotation;
        rot.x = 0.0f;
        rot.z = 0.0f;
        transform.rotation = rot;
    }
    
    private void RespawnWheelchair()
    {
        if (isWallCheck)
            transform.position = holder.transform.position;
    }

    #region 휠체어 SitOn / SitOff
    void SitOnWheelChair()
    {
        isSit = true;
        ui.SetActive(false);
        pv.RPC("SetParent", RpcTarget.All, NursingManager.Instance.userData.id);
        NursingManager.Instance.character.SitOnWheelChair(sitOffPos, SitOffWheelChair);
    }

    void SitOffWheelChair()
    {
        isSit = false;
        ui.SetActive(true);
    }
    #endregion

    public bool SitCheckOption()
    {
        if (isUp && !isLock && !isSit)
            return true;
        return false;
    }
    
    #region 애니메이션 동기화 - 발판, 자물쇠, 자물쇠UI
    void UpWheelChairFoot(string _type, bool _val)
    {
        pv.RPC("OnBoolAnimFoot", RpcTarget.All, _type, _val);
    }

    [PunRPC]
    void OnBoolAnimFoot(string _type, bool _val)
    {
        isUp = _val;
        animator_Foot.SetBool(_type, _val);
    }

    void OpenWheelCahirLock(string _type, bool _val)
    {
        pv.RPC("OnBoolAnimLock", RpcTarget.All, _type, _val);
    }

    [PunRPC]
    void OnBoolAnimLock(string _type, bool _val)
    {
        isLock = _val;
        //Debug.Log("isLock = " + _val);
        animator_Lock.SetBool(_type, _val);
    }

    void OpenWheelChairLockUI(string _type, bool _val)
    {
        pv.RPC("OnBoolAnimLockUI", RpcTarget.All, _type, _val);
    }

    [PunRPC]
    void OnBoolAnimLockUI(string _type, bool _val)
    {
        animator_LockUI.SetBool(_type, _val);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isUp);
            stream.SendNext(isLock);
            stream.SendNext(isSit);
        }
        else if (stream.IsReading)
        {
            isUp = (bool)stream.ReceiveNext();
            isLock = (bool)stream.ReceiveNext();
            isSit = (bool)stream.ReceiveNext();
        }
        InitWheelChairAnim();
    }
    #endregion

    public void UpdateData(bool _isSit, int userId)
    {
        isSit = _isSit;
        Scene.data.WheelCharItem_Data.IsSit = _isSit;
        Scene.data.WheelCharItem_Data.userId = userId;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
