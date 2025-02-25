using System;
using UnityEngine;

public abstract class Rooms : MonoBehaviour
{
    protected ContentsWorldUI contentsWorldUI;

    private void Awake()
    {
        contentsWorldUI = FindObjectOfType<ContentsWorldUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.GetComponent<CharacterManager>().PV.IsMine)
            Enter();
    }

    protected abstract void Enter();

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && other.GetComponent<CharacterManager>().PV.IsMine)
            Exit();
    }
    
    protected abstract void Exit();
}
