using System.Collections;
using System.Collections.Generic;
using Agora.Rtc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShareList : ConferenceWorldList
{
    [SerializeField] TMP_Text title;
    [SerializeField] Toggle audioToggle;
    
    private int display_num = 1;
    private string item_DisplayName_Type;

    private List<GameObject> tempList = new List<GameObject>();
    
    // enable: input, 움직임을 비활성화합니다.
    public override void OnEnable()
    {
        base.OnEnable();
        InputManager.Instance.isLock = true;
        NursingManager.Instance.character.SetMoveState(false);
        item_DisplayName_Type = LocalizeManager.Instance.GetString("display"); // 화면
        UpdateData();
    }

    // disable: input, 움직임을 활성화합니다.
    public override void OnDisable()
    {
        base.OnDisable();
        InputManager.Instance.isLock = false;
        NursingManager.Instance.character.SetMoveState(true);
    }
    
    // 공유 가능한 스크린 목록을 불러옵니다.
    private IEnumerator SetData()
    {
        var info =  AgoraChatManager.Instance.GetScreenCaptureSources();
        
        string item_displayName;
        
        tempList.Clear();

        for (int i = 0; i < info.Length; i++)
        {
            ScreenCaptureSourceInfo sourceInfo = info[i];

            if (sourceInfo.type == ScreenCaptureSourceType.ScreenCaptureSourceType_Custom ||
                sourceInfo.type == ScreenCaptureSourceType.ScreenCaptureSourceType_Unknown)
                continue;

            if (sourceInfo.sourceName.Contains("NVIDIA") || sourceInfo.sourceTitle.Contains("NVIDIA"))
                continue;

            if (string.IsNullOrEmpty(sourceInfo.sourceName) || string.IsNullOrEmpty(sourceInfo.sourceTitle))
                continue;
            
            GameObject temp = Instantiate(item, contents);
            
            Texture2D thumbTexture = new Texture2D((int)sourceInfo.thumbImage.width, (int)sourceInfo.thumbImage.height, TextureFormat.RGBA32, false);
            thumbTexture.LoadRawTextureData(sourceInfo.thumbImage.buffer);
            thumbTexture.Apply();

            temp.GetComponentInChildren<RawImage>().texture = thumbTexture;

            if (sourceInfo.type == ScreenCaptureSourceType.ScreenCaptureSourceType_Screen)
            {
                item_displayName = $"{item_DisplayName_Type} {display_num}";
                display_num++;

                temp.GetComponentInChildren<TMP_Text>().text = item_displayName;
            }
            else
                temp.GetComponentInChildren<TMP_Text>().text = sourceInfo.sourceTitle;

            string displayName = AgoraChatManager.Instance.GetDisplayName(sourceInfo);
            Toggle toggle = temp.GetComponent<Toggle>();
           
            toggle.SetIsOnWithoutNotify(AgoraChatManager.Instance.currentDisplayName == displayName);
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener((isOn) => { OnShareToggle(displayName); });
            temp.SetActive(true);
            tempList.Add(temp);
            yield return null;
        }

        yield return null;
    }
    
    // 공유할 스크린을 선택했을 경우
    private void OnShareToggle(string displayName)
    {
        UIManager.Instance.OpenPopup(LocalizeManager.Instance.GetString("changeScreenQuestion"),null, ()=> { OnShare(displayName); }); // 화면을 변경하시겠어요?
    }
    
    private void OnShare(string displayName)
    {
        AgoraChatManager.Instance.UpdateScreenShare(displayName, audioToggle.isOn);
        OnClose();
    }

    // 공유 스크린 목록을 최신으로 갱신합니다.
    public void UpdateData()
    {
        foreach (var temp in tempList)
            Destroy(temp);
        tempList.Clear();
        StartCoroutine(SetData());
    }
}
