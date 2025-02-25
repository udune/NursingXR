using System;
using TMPro;
using UnityEngine;

public class CartInventoryUI : MonoBehaviour
{
    [SerializeField] CartIcon[] cartIcons;
    [SerializeField] TMP_Text nameTxt;
    [SerializeField] RectTransform iconSelector;

    private float timeValue;
    private float timeTarget;

    private float posValue = 70;
    private float posTarget = 70;

    private bool isEnterRoom;
    private bool isEnterCartRange;
    private bool isHold;

    public int Index;
    
    private ContentsWorldUI contentsWorldUI;

    private void Awake()
    {
        contentsWorldUI = FindObjectOfType<ContentsWorldUI>();
        GetComponent<RectTransform>().anchoredPosition = new Vector2(250.0f, -160.0f);
    }

    private void Start()
    {
        Index = -2;
    }

    private void Update()
    {
        MoveHeight();
        MoveSelector();
    }

    private void MoveHeight()
    {
        if (!Mathf.Approximately(timeValue, timeTarget))
        {
            timeValue = Mathf.MoveTowards(timeValue, timeTarget, Time.deltaTime * 2);
            GetComponent<RectTransform>().anchoredPosition = new Vector2(250.0f, -160 + BackEaseOut(timeValue, 0, 1, 1) * 190);
        }
    }

    private void MoveSelector()
    {
        UpdatePosTarget();
        if (!Mathf.Approximately(posValue, posTarget))
        {
            posValue = Mathf.MoveTowards(posValue, posTarget, Time.deltaTime * 1000);
            iconSelector.anchoredPosition = new Vector2(posValue, -50);
        }
    }

    public void OnCheckIndex(int index)
    {
        if (index == Index)
            OnPrev();
    }

    public void OnPrev()
    {
        UpdateIndex(PrevIndex());
        UpdateCartList();
    }

    public void OnNext()
    {
        UpdateIndex(NextIndex());
        UpdateCartList();
    }

    public void OnReset()
    {
        UpdateIndex(-1);
        UpdateCartList();
    }

    public void UpdateIndex(int i)
    {
        IconSetActive(false);
        Index = i;
        IconSetActive(true);
    }

    public void UpdateCartList()
    {
        UpdateText();
        UpdateTooltip();
    }

    private void UpdatePosTarget()
    {
        posTarget = Index > -1 ? cartIcons[Index].GetPositionX() : 60;
    }

    // 카트 UI 물품 이름을 변경합니다.
    private void UpdateText()
    {
        nameTxt.text = Index > -1 ? cartIcons[Index].name : LocalizeManager.Instance.GetString("empty");
    }

    private void UpdateTooltip()
    {
        if (Index == -2)
            return;
        
        if (Index > -1)
            contentsWorldUI.toolTip.SetTitle(cartIcons[Index].Title);
        else
        {
            contentsWorldUI.toolTip.SetTitle(string.Empty);
            contentsWorldUI.toolTip.Close();
        }
    }

    private void IconSetActive(bool value)
    {
        if (Index > -1 && Index < cartIcons.Length)
            cartIcons[Index].SetCartObjectActive(value);
    }

    private int PrevIndex()
    {
        Index = Index == -1 ? cartIcons.Length : Index;
        for (int i = Index - 1; i >= 0; i--)
        {
            if (cartIcons[i].GetIconActive())
                return i;
        }
        return -1;
    }

    private int NextIndex()
    {
        for (int i = Index + 1; i < cartIcons.Length; i++)
        {
            if (cartIcons[i].GetIconActive())
                return i;
        }
        return -1;
    }

    private float BackEaseOut(float t, float b, float c, float d)
    {
        return c * ((t = t / d - 1) * t * ((1.7f + 1) * t + 1.7f) + 1) + b;
    }

    public void EnterRoom()
    {
        isEnterRoom = true;
        UpdateTimeTarget();
    }

    public void ExitRoom()
    {
        isEnterRoom = false;
        UpdateTimeTarget();
    }

    public void EnterCartRange()
    {
        isEnterCartRange = true;
        UpdateTimeTarget();
    }

    public void ExitCartRange()
    {
        isEnterCartRange = false;
        UpdateTimeTarget();
    }

    public void HoldCart()
    {
        isHold = true;
    }

    public void DropCart()
    {
        isHold = false;
    }

    private void UpdateTimeTarget()
    {
        timeTarget = !isHold && (isEnterRoom || isEnterCartRange) ? 1 : 0;
    }
}

