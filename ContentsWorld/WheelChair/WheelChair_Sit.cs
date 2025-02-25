using Photon.Pun;
using UnityEngine;

public class WheelChair_Sit : InteractionWheelChair
{
    [SerializeField] GameObject ui;

    // 클릭했을때
    public override void OnPointerDown()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerDown();

        if (wheelChair.SitCheckOption())
            SitOnWheelChair();
    }
    public override void OnPointerEnter()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerEnter();
        if (wheelChair.SitCheckOption())
            ui.SetActive(true);
    }
    public override void OnPointerExit()
    {
        if (wheelChair.isObserver) return;
        base.OnPointerExit();

        ui.SetActive(false);
    }
    // 휠체어 앉기
    public void SitOnWheelChair()
    {
        wheelChair.ChangeWheelChairOption(WheelChairOption.SitOn);
        ui.SetActive(false);
    }
    // 휠체어 일어서기
    void SitOffWheelChair()
    {
        ui.SetActive(false);
        wheelChair.ChangeWheelChairOption(WheelChairOption.SitOff);
    }

}
