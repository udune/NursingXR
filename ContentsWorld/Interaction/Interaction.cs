using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interaction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected PhotonView pv;
    protected ContentsWorldUI contentsWorldUI;
    protected ContentsWorldScene Scene;
    protected CameraManager camera;
    
    // 힌트 팝업을 활성화/비활성화합니다.
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

    [SerializeField] public Outline[] outlines;

    private void Awake()
    {
        AwakeAction();
    }

    protected virtual void AwakeAction()
    {
        pv = GetComponent<PhotonView>();
        if (pv != null)
            pv.OwnershipTransfer = OwnershipOption.Takeover; 

        Scene = FindObjectOfType<ContentsWorldScene>();
        contentsWorldUI = FindObjectOfType<ContentsWorldUI>();
        camera = FindObjectOfType<CameraManager>();
    }

    private void Start()
    {
        StartAction();
    }
    
    protected virtual void StartAction() { }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (pv != null)
            pv.RequestOwnership();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (outlines == null) return;
        foreach (Outline outline in outlines)
            DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 3.0f, 0.25f);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (outlines == null) return;
        foreach (Outline outline in outlines)
            DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 0.0f, 0.25f);
    }

    // 불러온 포톤 데이터를 오브젝트에 적용합니다.
    public virtual void LoadWorld() { }

    // 포톤 데이터를 업데이트 합니다.
    public virtual void UpdateData() { }
    public virtual void UpdateData_Item() { }
    public virtual void UpdateData_Rope() { }
    public virtual void UpdateData(bool active) { }
    public virtual void UpdateData(int index) { }
}
