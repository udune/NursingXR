using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicItem : MonoBehaviour
{
    [SerializeField] TMP_Text username;
    [SerializeField] Toggle micToggle;
    
    // 해당 마이크 아이템을 설정합니다.
    public void SetMic(PhotonView pv)
    {
        username.text = pv.Controller.NickName;
        micToggle.SetIsOnWithoutNotify(!pv.GetComponentInChildren<AudioSource>().mute);
        micToggle.onValueChanged.RemoveAllListeners();
        micToggle.onValueChanged.AddListener((on) => pv.GetComponent<CharacterManager>().OnRequestMic(on));
    }
}
