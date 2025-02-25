using Photon.Pun;

public class DeliveryRoom : Rooms
{
    protected override void Enter()
    {
        contentsWorldUI.roomPanel.SetText($"[{LocalizeManager.Instance.GetString("deliveryRoom")}] {PhotonNetwork.CurrentRoom.Name}"); // 분만실
    }

    protected override void Exit()
    {
        contentsWorldUI.toolTip.SetTooltip("");
    }
}
