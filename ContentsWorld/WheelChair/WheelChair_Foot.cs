using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class WheelChair_Foot : InteractionWheelChair
{
    
    public override void OnPointerDown()
    {
        // if (wheelChair.isObserver) return;

        // if (wheelChair.isUp)
        //     Down();
        // else
        //     Up();
    }
    // 휠체어 발판 내림
    private void Down()
    {
        // if(!wheelChair.isSit)
        //     wheelChair.ChangeWheelChairOption(WheelChairOption.footDown);
    }
    // 휠체어 발판 올림
    private void Up()
    {
        // wheelChair.ChangeWheelChairOption(WheelChairOption.footUP);
    }

    public override void OnPointerEnter()
    {
        // if (wheelChair.isObserver) return;
        // base.OnPointerEnter();
        // if (wheelChair.isUp)
        //     wheelChair.InfoMessage(LocalizeManager.Instance.GetString("lowerFootrest")); // 휠체어 발판 내림
        // else
        //     wheelChair.InfoMessage(LocalizeManager.Instance.GetString("raiseFootrest")); // 휠체어 발판 올림
    }

    public override void OnPointerExit()
    {
        // if (wheelChair.isObserver) return;
        // base.OnPointerExit();
        // wheelChair.InfoMessage(""); // 안내 메세지 비활성화
    }

}
