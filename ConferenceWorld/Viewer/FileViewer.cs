using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileViewer : ConferenceWorldList
{
    [Header("Button")]
    [SerializeField] GameObject downloadBtn;
    protected string fileName;
    protected string filePath;

    public virtual void Awake()
    {
        downloadBtn.SetActive(false);
    }
    
    // viewer에 내용(FileName, FilePath)을 보여줍니다.
    public virtual void SetViewer(string filePath, string fileName)
    {
        downloadBtn.SetActive(true);
        this.fileName = fileName;
        this.filePath = filePath;
    }
    
    // 파일을 다운로드합니다.
    public void OnDownload()
    {
        StandaloneFileBrowser.OpenFolderPanelAsync("Select the folder path to save", null, false, SelectCallback); // 저장할 폴더 경로를 선택하세요.
    }
    
    private void SelectCallback(IList<ItemWithStream> result)
    {
        if (result != null)
        {
            string savePath = result[0].Name;
            if (!string.IsNullOrEmpty(savePath))
                StartCoroutine(DownloadFile(filePath, $"{savePath}/{fileName}{Path.GetExtension(filePath)}"));
        }
    }
    
    // 파일을 다운로드합니다.
    private IEnumerator DownloadFile(string url, string savePath)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
            Debug.Log(www.error);
        else
        {
            File.WriteAllBytes(savePath, www.downloadHandler.data);
            UIManager.Instance.OpenToast(LocalizeManager.Instance.GetString("downloadSuccess")); // 파일 다운로드 성공.
        }
    }
}
