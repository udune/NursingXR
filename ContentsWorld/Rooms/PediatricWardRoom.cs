using Photon.Pun;

public class PediatricWardRoom : Rooms
{
    protected override void Enter()
    {
        contentsWorldUI.roomPanel.SetText($"[{LocalizeManager.Instance.GetString("pediatricWard")}] {PhotonNetwork.CurrentRoom.Name}"); // 아동병실
        contentsWorldUI.toolTip.SetTitle("Tip");
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("PediatricWardRoomTooltip")); // 카트를 확인하고 물품을 환자에게 적용하세요.
    }

    protected override void Exit()
    {
        contentsWorldUI.toolTip.SetTooltip("");
    }
}
