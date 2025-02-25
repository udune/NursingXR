using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RoomPanel : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    private int keyOpen;

    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        keyOpen = Animator.StringToHash("Open");
    }

    public void SetText(string text)
    {
        nameText.text = text;
        animator.SetTrigger(keyOpen);
    }
}
