using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

public class MemoItem : MonoBehaviour
{
    [SerializeField] TMP_Text username;
    [SerializeField] TMP_Text memo;
    private int idx;
    private string path;
    
    // username, memo 내용을 저장합니다.
    public void SetMemo(string username, string memo)
    {
        this.username.text = username;
        this.memo.text = memo;
    }

    // 파일을 다운로드합니다.
    public void SaveText()
    {
        StandaloneFileBrowser.OpenFolderPanelAsync("Select the folder path to save", null, false, SelectCallback);
    }
    
    // 파일을 다운로드합니다.
    private void SelectCallback(IList<ItemWithStream> result)
    {
        if (result != null)
        {
            string savePath = result[0].Name;
            if (!string.IsNullOrEmpty(savePath))
            {
                do
                {
                    path = $"{savePath}/{memo.text.Substring(0, 5)}_{username.text}_{NursingManager.Instance.userData.username}_{DateTime.Now:yyyy-M-d}_{idx}.txt";
                    if (!File.Exists(path))
                    {
                        var file = File.CreateText(path);
                        file.Close();
                        break;
                    }
                    idx++;
                } while (true);
                
                File.WriteAllText(path, memo.text);
                UIManager.Instance.OpenToast(LocalizeManager.Instance.GetString("downloadSuccess")); // 파일 다운로드 성공.
            }
        }
    }
}
