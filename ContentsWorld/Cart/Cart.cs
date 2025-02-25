using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Cart : Interaction
{
    private bool isHolding;
    private float yDir;
    
    private Vector3 syncPos;
    private Quaternion syncRot;

    private GameObject holder;

    private const string Hold = "Hold";
    private const string UI = "UI";
    
    [SerializeField] Outline wallcheck_outline;
    [SerializeField] public GameObject UI_Go;
    [SerializeField] public GameObject UI_Zoom_AED_Go;
    [SerializeField] public GameObject UI_Zoom_SPO_Go;
    [SerializeField] public Button UI_Next_Btn;
    [SerializeField] public Button UI_Prev_Btn;
    [SerializeField] public Button UI_ZoomIn_AED_Btn;
    [SerializeField] public Button UI_ZoomOut_AED_Btn;
    [SerializeField] public Button UI_ZoomIn_SPO_Btn;
    [SerializeField] public Button UI_ZoomOut_SPO_Btn;
    private bool isVR;
    private bool isWallCheck;
    
    [PunRPC]
    public void ContentsWorld_CartEvent(string type, bool isTrue, int actorNum)
    {
        if (type == "UI")
        {
            if (isTrue)
                contentsWorldUI.cartInventoryUI.OnNext();
            else
                contentsWorldUI.cartInventoryUI.OnPrev();

            // 현재 선택된 카트 UI 물품을 포톤 데이터에 저장합니다.
            UpdateData(contentsWorldUI.cartInventoryUI.Index);
        }
        else if (type == "Hold")
        {
            // 카트를 잡은 플레이어 오브젝트를 찾습니다.
            holder = PhotonManager.Instance.FindCharacter(actorNum);
            
            if (isTrue)
            {
                isHolding = true;
                
                UI_Go.SetActive(false);
                if (Scene.aed_cart.gameObject.activeSelf) UI_Zoom_AED_Go.SetActive(false);
                if (Scene.spo_cart.gameObject.activeSelf) UI_Zoom_SPO_Go.SetActive(false);
                
                contentsWorldUI.cartInventoryUI.HoldCart();
            }
            else
            {
                isHolding = false;
                
                UI_Go.SetActive(true);
                if (Scene.aed_cart.gameObject.activeSelf) UI_Zoom_AED_Go.SetActive(true);
                if (Scene.spo_cart.gameObject.activeSelf) UI_Zoom_SPO_Go.SetActive(true);
                
                LookAtPlayer();
                RespawnCart();
                contentsWorldUI.cartInventoryUI.DropCart();
            }
        }
    }

    private void Update()
    {
        // 벽 안으로 들어갔을때 흰색 아웃라인을 활성화합니다.
        isWallCheck = Physics.OverlapSphere(transform.position + (Vector3.up * 1.0f), 0.6f, 1 << 8).Length > 0;
        wallcheck_outline.enabled = isWallCheck;
        
        if (isHolding)
        {
            if (Input.GetMouseButton(0))
                MoveCart();
        }
    }

    // 카트를 잡았을 때
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver || InputManager.Instance.isAED || InputManager.Instance.isSpo || Scene.IsInteraction) return;
        RequestSendEvent(Hold, true);
    }
    
    // 카트를 놓았을 때
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Scene.character.isObserver) return;
        RequestSendEvent(Hold, false);
    }
    
    // 카트 UI 다음 버튼
    public void NextBtn()
    {
        RequestSendEvent(UI, true);
    }
    
    // 카트 UI 이전 버튼
    public void PrevBtn()
    {
        RequestSendEvent(UI, false);
    }

    // AED 물품을 줌인합니다.
    public void ZoomIn_AED()
    {
        if (InputManager.Instance.isVR)
        {
            isVR = true; 
            camera.SwitchInterface();
        }
        
        NursingManager.Instance.character.SetMoveState(false);
        UI_ZoomIn_AED_Btn.interactable = false;
        UI_ZoomOut_AED_Btn.interactable = true;
        InputManager.Instance.isAED = true;
    }

    // AED 물품을 줌아웃합니다.
    public void ZoomOut_AED()
    {
        if (isVR)
        {
            camera.SwitchInterface(); 
            isVR = false;
        }
        
        NursingManager.Instance.character.SetMoveState(true);
        camera.ResetPerspectiveCamera();
        UI_ZoomOut_AED_Btn.interactable = false;
        UI_ZoomIn_AED_Btn.interactable = true;
        InputManager.Instance.isAED = false;
    }

    // SPO 물품을 줌인합니다.
    public void ZoomIn_SPO()
    {
        if (InputManager.Instance.isVR)
        {
            isVR = true; 
            camera.SwitchInterface();
        }
        
        NursingManager.Instance.character.SetMoveState(false);
        UI_ZoomIn_SPO_Btn.interactable = false;
        UI_ZoomOut_SPO_Btn.interactable = true;
        InputManager.Instance.isSpo = true;
    }

    // SPO 물품을 줌아웃합니다.
    public void ZoomOut_SPO()
    {
        if (isVR)
        {
            camera.SwitchInterface(); 
            isVR = false;
        }
        
        NursingManager.Instance.character.SetMoveState(true);
        camera.ResetPerspectiveCamera();
        UI_ZoomOut_SPO_Btn.interactable = false;
        UI_ZoomIn_SPO_Btn.interactable = true;
        InputManager.Instance.isSpo = false;
    }

    // 카트 동작 포톤 RPC 요청합니다.
    private void RequestSendEvent(string type, bool isTrue)
    {
        pv.RPC("ContentsWorld_CartEvent", RpcTarget.All, type, isTrue, PhotonNetwork.LocalPlayer.ActorNumber);
    }

    // 카트를 회전시킵니다.
    private void RotateCart()
    {
        if (Input.GetAxis("Mouse X") < 0)
            yDir = 1.0f;
        else if (Input.GetAxis("Mouse X") > 0)
            yDir = -1.0f;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, transform.localEulerAngles.y + 3 * yDir, 0.0f));
    }

    private void LookAtPlayer()
    {
        transform.forward = -holder.transform.forward;
    }

    private void RespawnCart()
    {
        if (isWallCheck)
            transform.position = holder.transform.position;
    }
    
    // 카트를 캐릭터 앞에서 움직입니다.
    private void MoveCart()
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

    // 불러온 포톤 데이터를 카트UI에 적용합니다.
    public override void LoadWorld()
    {
        contentsWorldUI.cartInventoryUI.Index = Scene.data.CartUI_Index;
        contentsWorldUI.cartInventoryUI.UpdateIndex(Scene.data.CartUI_Index);
    }

    // 포톤 데이터를 업데이트합니다.
    public override void UpdateData(int index)
    {
        base.UpdateData(index);
        Scene.data.CartUI_Index = index;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
