using System;
using System.Collections;
using Constants;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class AED : Interaction_Item
{
    [SerializeField] public Interaction_Item interaction_Items;
    [SerializeField] Collider[] colliders;
    [SerializeField] AED_Display display;
    [SerializeField] public AED_Power power;
    [SerializeField] public AED_Charge charge;
    [SerializeField] public AED_Shock shock;
    [SerializeField] GameObject model;
    [SerializeField] GameObject zoomUI;
    
    private AudioSource audio;
    
    protected override void AwakeAction()
    {
        base.AwakeAction();
        audio = GetComponent<AudioSource>();
        OnWaitPower();
    }

    private void OnEnable()
    {
        zoomUI.SetActive(true);
    }

    private void OnDisable()
    {
        zoomUI.SetActive(false);
        if (NursingManager.Instance.character != null) NursingManager.Instance.character.SetMoveState(true);
        CameraManager camera = FindObjectOfType<CameraManager>();
        if (camera != null) camera.ResetPerspectiveCamera();
        InputManager.Instance.isAED = false;
        if (Scene != null)
        {
            Scene.cart.UI_ZoomOut_AED_Btn.interactable = false;
            Scene.cart.UI_ZoomIn_AED_Btn.interactable = true;
        }
    }

    private void OnWaitPower()
    {
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("aedDevicePowerOn")); // 전원을 켜 제세동 에너지량(J)을 선택합니다.
    }
    
    public void OnWaitHandle()
    {
        SetCollider(1);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("paddleApplyToPatient")); // Paddle을 환자 가슴에 적용시키세요.
    }
    
    public void OnWaitCharge()
    {
        interaction_Items.gameObject.SetActive(false);
        SetCollider(2);
    
        display.TurnOn();
    }
    
    public void OnCharge()
    {
        SetCollider(-1);
    }
    
    public void OnWaitShock()
    {
        SetCollider(3);
        audio.PlayOneShot(audio.clip);
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("shockClick")); // Shock 버튼을 클릭하세요.
    }
    
    public void OnShock()
    {
        audio.Stop();
    
        interaction_Items.gameObject.SetActive(false);
        SetCollider(3);
    
        display.ChangeDisp();
    }
    
    private void SetCollider(params int[] nums)
    {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = Contains(i, nums);
    }
    
    private bool Contains(int n, int[] nums)
    {
        foreach (var num in nums)
        {
            if (num == n)
                return true;
        }
    
        return false;
    }
}
