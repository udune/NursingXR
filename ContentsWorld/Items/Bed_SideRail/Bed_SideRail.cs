using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bed_SideRail : Interaction
{
    enum Type
    {
        Left,
        Right
    }

    [SerializeField] Type type;
    [SerializeField] Transform[] siderails;
    public BoxCollider[] colliders;

    private float value;
    public float target;
    public bool up = true;

    [PunRPC]
    public void ContentsWorld_SideRail(bool isUp)
    {
        if (isUp)
            Down();
        else
            Up();

        UpdateData();
    }
    
    protected override void AwakeAction()
    {
        base.AwakeAction();
        colliders = GetComponents<BoxCollider>();
        Up();
    }

    private void Update()
    {
        if (!Mathf.Approximately(value, target))
        {
            value = Mathf.MoveTowards(value, target, Time.deltaTime * 2.5f);

            siderails[0].localPosition = new Vector3(
                0.5f,
                0.4f + Mathf.Cos(value * 75 * Mathf.Deg2Rad) * 0.35f,
                -0.14f + Mathf.Sin(value * 75 * Mathf.Deg2Rad) * 0.375f);

            for (int i = 1; i < siderails.Length - 1; i++)
            {
                siderails[i].localRotation = Quaternion.Euler(-90 + 75 * value, 0, 0);
            }
            siderails[siderails.Length - 1].localRotation = Quaternion.Euler(75 * value, 0, 0);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        pv.RPC("ContentsWorld_SideRail", RpcTarget.All, up);
    }

    private void Down()
    {
        up = false;
        target = 1;
        colliders[0].enabled = false;
        colliders[1].enabled = true;
    }

    private void Up()
    {
        up = true;
        target = 0;
        colliders[0].enabled = true;
        colliders[1].enabled = false;
    }
    
    public override void UpdateData()
    {
        if (type == Type.Left)
        {
            Scene.data.Room_Data.Bed_SideRail_Data.Bed_SideRail_Left_Up = up;
            Scene.data.Room_Data.Bed_SideRail_Data.Bed_SideRail_Left_Target = target;
        }
        else if (type == Type.Right)
        {
            Scene.data.Room_Data.Bed_SideRail_Data.Bed_SideRail_Right_Up = up;
            Scene.data.Room_Data.Bed_SideRail_Data.Bed_SideRail_Right_Target = target;
        }
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
