using System;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class Door : Interaction
{
    enum Direction
    {
        X,
        Y,
        Z
    }

    [SerializeField] Direction direction;
    [SerializeField] Transform door_Left;
    [SerializeField] Transform door_Right;

    private float init_Left;
    private float init_Right;

    private Collider collider;

    protected override void AwakeAction()
    {
        base.AwakeAction();
        collider = GetComponent<Collider>();
        switch (direction)
        {
            case Direction.X:
                break;
            case Direction.Y:
                break;
            case Direction.Z:
                init_Left = door_Left.localPosition.z;
                init_Right = door_Right.localPosition.z;
                break;
        }
    }

    [PunRPC]
    public void ContentsWorld_Door(bool isOpen, float leftTarget, float rightTarget)
    {
        if (isOpen)
        {
            collider.enabled = true;
            switch (direction)
            {
                case Direction.X:
                    break;
                case Direction.Y:
                    break;
                case Direction.Z:
                    door_Left.DOLocalMoveZ(leftTarget, 0.5f);
                    door_Right.DOLocalMoveZ(rightTarget, 0.5f);
                    break;
            }
        }
        else
        {
            collider.enabled = false;
            switch (direction)
            {
                case Direction.X:
                    break;
                case Direction.Y:
                    break;
                case Direction.Z:
                    door_Left.DOLocalMoveZ(leftTarget, 0.5f);
                    door_Right.DOLocalMoveZ(rightTarget, 0.5f);
                    break;
            }
            collider.enabled = true;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            pv.RPC("ContentsWorld_Door", RpcTarget.All, true, init_Left - 0.8f, init_Right + 0.8f);
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            pv.RPC("ContentsWorld_Door", RpcTarget.All, false, init_Left, init_Right);
    }
}
