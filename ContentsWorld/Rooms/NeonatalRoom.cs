using Photon.Pun;

public class NeonatalRoom : Rooms
{
    protected override void Enter()
    {
        contentsWorldUI.roomPanel.SetText($"[{LocalizeManager.Instance.GetString("neonatalRoom")}] {PhotonNetwork.CurrentRoom.Name}"); // 신생아실
    }

    protected override void Exit()
    {
        contentsWorldUI.toolTip.SetTooltip("");
    }
}
