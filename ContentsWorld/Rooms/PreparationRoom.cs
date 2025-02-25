using Photon.Pun;

public class PreparationRoom : Rooms
{
    protected override void Enter()
    {
        contentsWorldUI.roomPanel.SetText($"[{LocalizeManager.Instance.GetString("preparationRoom")}] {PhotonNetwork.CurrentRoom.Name}"); // 물품준비실
        contentsWorldUI.cartInventoryUI.EnterRoom();
        contentsWorldUI.toolTip.SetTitle("Tip");
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("PreparationRoomTooltip")); // 카트를 확인하고 물품을 클릭해서 추가하거나 삭제하세요.
    }

    protected override void Exit()
    {
        contentsWorldUI.cartInventoryUI.ExitRoom();
        contentsWorldUI.toolTip.SetTooltip("");
    }
}
