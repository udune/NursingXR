using Photon.Pun;

public class CharacterSelectRoom : Rooms
{
    protected override void Enter()
    {
        if (PhotonNetwork.IsConnected)
            contentsWorldUI.roomPanel.SetText($"[{LocalizeManager.Instance.GetString("avatarRoom")}] {PhotonNetwork.CurrentRoom.Name}"); // 아바타 룸
        contentsWorldUI.toolTip.SetTitle("Tip");
        contentsWorldUI.toolTip.SetTooltip(LocalizeManager.Instance.GetString("ContentsWorldTooltip")); // 아바타를 선택하고 물품 준비실로 가서 카트를 확인하세요.
    }

    protected override void Exit()
    {
        contentsWorldUI.toolTip.SetTooltip("");
    }
}
