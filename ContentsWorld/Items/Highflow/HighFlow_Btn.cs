using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighFlow_Btn : Interaction
{
    [SerializeField] HighFlow highFlow;
    [SerializeField] Oxygen oxygen;
    [SerializeField] MeshRenderer disp;
    [SerializeField] Material[] materials;

    public bool on;
    public float time;
    public bool loading;
    
    private AudioSource audio;

    [PunRPC]
    public void ContentsWorld_HighflowButton(bool isOn)
    {
        audio.PlayOneShot(audio.clip);

        if (isOn)
            PowerOff();
        else
            PowerOn();
        
        UpdateData();
    }

    protected override void AwakeAction()
    {
        base.AwakeAction();
        audio = GetComponent<AudioSource>();
    }

    protected override void StartAction()
    {
        PowerOff();
    }

    public void OnDisable()
    {
        audio.Stop();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (Scene.character.isObserver) return;
        if (highFlow.IsRope_Mount && oxygen.IsRope_Mount)
            pv.RPC("ContentsWorld_HighflowButton", RpcTarget.All, on);
    }

    private void Update()
    {
        if (!on)
            return;

        if (time < 8)
        {
            time += Time.deltaTime;

            if (!loading && time > 4)
                Loading();
        }
        else
            Complete();
    }

    private void PowerOff()
    {
        on = false;
        SetMaterial(0);
    }

    private void PowerOn()
    {
        time = 0;
        on = true;
        loading = false;
        GetComponent<Collider>().enabled = false;
        SetMaterial(1);
    }

    private void Loading()
    {
        loading = true;
        SetMaterial(2);
    }

    private void Complete()
    {
        GetComponent<Collider>().enabled = true;
        SetMaterial(3);
    }

    public void SetMaterial(int n)
    {
        disp.material = materials[n];
    }
    
    public override void UpdateData()
    {
        Scene.data.CartItem_Data.HighFlow_Data.HighFlow_Btn_On = on;
        Scene.data.CartItem_Data.HighFlow_Data.HighFlow_Btn_Time = time;
        Scene.data.CartItem_Data.HighFlow_Data.HighFlow_Btn_Loading = loading;
        
        if (pv.IsMine)
            Scene.SavePhotonData();
    }
}
