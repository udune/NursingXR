using System;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;

public class Monitor : MonoBehaviour
{
    [SerializeField] CanvasWebViewPrefab webview;
    [SerializeField] RawImage view; 

    public void OnClick()
    {
        UIWebview ui = UIManager.Instance.GetUI<UIWebview>();
        ui.OpenUI();
        
        ui.SetMonitor(this);
    }

    public void SetView(RawImage view_ui)
    {
        view.texture = view_ui.texture;
    }
}
