using Photon.Pun;
using UnityEngine;

public class WheelChair_Lock : InteractionWheelChair
{

    public override void OnPointerDown()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerDown();

        if (wheelChair.isLock)
            UnLock();
        else
            Lock();
    }

    public override void OnPointerEnter()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerEnter();
        if (wheelChair.isLock)
            wheelChair.InfoMessage(LocalizeManager.Instance.GetString("unlock")); // 잠금 해제
        else
            wheelChair.InfoMessage(LocalizeManager.Instance.GetString("lock")); // 잠금
    }

    public override void OnPointerExit()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerExit();
        wheelChair.InfoMessage(""); // 안내 메세지 비활성화
    }
    // 자물쇠 잠금 해제
    private void UnLock()
    {
        wheelChair.ChangeWheelChairOption(WheelChairOption.UnLock);
    }
    // 자물쇠 잠금
    private void Lock()
    {
        if(!wheelChair.isSit)
            wheelChair.ChangeWheelChairOption(WheelChairOption.Lock); 
    }
}
