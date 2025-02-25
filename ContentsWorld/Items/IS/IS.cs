using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class IS : Interaction
{
    [SerializeField] Transform[] balls;
    [SerializeField] float target;
    [SerializeField] GameObject[] models;
    
    private bool on;
    private float value;

    [PunRPC]
    public void ContentsWorld_IS(bool power)
    {
        if (power)
        {
            PowerOn();
        }
        else
        {
            PowerOff();
            UpdateData();
        }
    }

    private void OnEnable()
    {
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("devicePressOn")); // 장치를 눌러 작동시키세요.
    }

    public void OnDisable()
    {
        value = 0;
        PowerOff();
        UpdateBalls();
    }
    
    private void Update()
    {
        if (!Mathf.Approximately(value, target))
            value = Mathf.MoveTowards(value, target, Time.deltaTime * (on ? 0.5f : 1.5f));
        
        UpdateBalls();
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Scene.character.isObserver) return;
        base.OnPointerDown(eventData);
        pv.RPC("ContentsWorld_IS", RpcTarget.All, true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        pv.RPC("ContentsWorld_IS", RpcTarget.All, false);
    }

    private void PowerOn()
    {
        on = true;
        target = 1;
    }

    private void PowerOff()
    {
        on = false;
        target = 0;
    }

    private void UpdateBalls()
    {
        balls[0].localPosition = new Vector3(-0.1175f, 0, 0.05f + value * (0.15f + Mathf.Sin(Time.time * 2) * 0.01f));
        balls[1].localPosition = new Vector3(0, 0, 0.05f + value * (0.3f + Mathf.Sin(Time.time * 3) * 0.015f));
        balls[2].localPosition = new Vector3(0.1175f, 0, 0.05f + value * (0.45f + Mathf.Sin(Time.time * 4) * 0.02f));
    }
}
