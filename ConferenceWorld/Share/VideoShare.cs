using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;

public class VideoShare : MonoBehaviour
{
    [SerializeField] CanvasWebViewPrefab webview;
    [SerializeField] TMP_Text username;
    [SerializeField] Button fullBtn;
    [SerializeField] Sprite full;
    [SerializeField] Sprite zoomout;

    private bool isFull;
    
    private Vector2 anchoredPos;
    private Vector2 sizeDelta;
    private Vector2 anchorMin;
    private Vector2 anchorMax;
    private Vector2 pivot;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        anchoredPos = rect.anchoredPosition;
        sizeDelta = rect.sizeDelta;
        anchorMin = rect.anchorMin;
        anchorMax = rect.anchorMax;
        pivot = rect.pivot;
    }

    private void OnEnable()
    {
        SetSize(anchoredPos, sizeDelta, anchorMin, anchorMax, pivot);
    }

    public void SetVideo(string username, string url)
    {
        this.username.text = username;
        webview.InitialUrl = url;
    }

    public void OnSize()
    {
        isFull = !isFull;
        if (isFull)
        {
            SetSize(Vector2.zero, Vector2.zero, Vector2.zero, Vector2.one, new(0.5f, 0.5f));
            fullBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(60.0f, 60.0f);
            fullBtn.transform.GetComponentInChildren<Image>().sprite = zoomout;
        }
        else
        {
            SetSize(anchoredPos, sizeDelta, anchorMin, anchorMax, pivot);
            fullBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(30.0f, 30.0f);
            fullBtn.transform.GetComponentInChildren<Image>().sprite = full;
        }
    }

    private void SetSize(Vector2 anchoredPos, Vector2 sizeDelta, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot)
    {
        rect.anchoredPosition = anchoredPos;
        rect.sizeDelta = sizeDelta;
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = pivot;
    }
}
