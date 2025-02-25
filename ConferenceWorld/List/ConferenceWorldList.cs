using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConferenceWorldList : MonoBehaviour
{
    protected ConferenceWorldScene Scene => FindObjectOfType<ConferenceWorldScene>();
    
    [SerializeField] protected Transform contents;
    [SerializeField] protected GameObject item;

    // enable: 리스트를 설정합니다. 하위 클래스에서 재정의
    public virtual void OnEnable()
    {
        SetList();
    }

    // disable: 하위 클래스에서 재정의
    public virtual void OnDisable()
    {
        
    }
    
    public void OnClose()
    {
        gameObject.SetActive(false);
    }

    protected virtual void SetList() { }
}
