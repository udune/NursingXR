using Photon.Pun;
using UnityEngine;

public class WheelChair_Push : InteractionWheelChair
{
    [SerializeField] WheelChair_Wheel wheel;
    [SerializeField] Outline wallcheck_outline;
    
    private void Update()
    {
        wheelChair.isWallCheck = Physics.OverlapSphere(transform.position + (Vector3.up * 0.4f), 0.6f, 1 << 8).Length > 0;
        wallcheck_outline.enabled = wheelChair.isWallCheck;
    }

    public override void OnPointerDown()
    {
        wheelChair.pv.RequestOwnership();
        if (wheelChair.isObserver) return;
        if (HoldCheckOption())
        {
            wheelChair.ui.SetActive(false);
            wheelChair.WheelchairPushEvent(true);
        }
    }

    public void OnPointerUp()
    {
        if (wheelChair.isObserver) return;
        wheelChair.ui.SetActive(true);
        wheelChair.WheelchairPushEvent(false);
    }
    
    private bool HoldCheckOption()
    {
        if (!wheelChair.isLock)
            return true;
        return false;
    }

    public override void OnPointerEnter()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerEnter();
        wheelChair.InfoMessage(LocalizeManager.Instance.GetString("wheelchairPush")); // 휠체어 밀기
    }

    public override void OnPointerExit()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerExit();
        wheelChair.InfoMessage(""); // 휠체어 밀기
    }
}
