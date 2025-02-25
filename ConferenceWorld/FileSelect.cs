using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Constants;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class FileSelect : MonoBehaviour
{
    ConferenceWorldScene Scene => FindObjectOfType<ConferenceWorldScene>();
    
    private ExtensionFilter[] extensionList_Image;
    private ExtensionFilter[] extensionList_PDF;
    private string type;
    
    // 선택 가능한 extension(파일 확장자)를 설정합니다.
    private void Awake()
    {
        extensionList_Image = new List<ExtensionFilter> { new("Image Files", new[] { "jpg", "jpeg", "png", "gif", "psd", "tif", "tiff" }) }.ToArray();
        extensionList_PDF = new List<ExtensionFilter> { new("PDF File", new[] { "pdf" }) }.ToArray();
    }
    
    // Enable: Input, 움직임을 비활성화합니다.
    public void OnEnable()
    {
        InputManager.Instance.isLock = true;
        NursingManager.Instance.character.SetMoveState(false);
    }

    // Disable: Input, 움직임을 활성화합니다.
    public void OnDisable()
    {
        InputManager.Instance.isLock = false;
        NursingManager.Instance.character.SetMoveState(true);
    }

    // 이미지 공유를 선택했을 경우
    public void OnImage()
    {
        type = "Image";
        StandaloneFileBrowser.OpenFilePanelAsync("Select the Image file to share.", null, extensionList_Image, false, SelectCallback); // 공유할 이미지 파일을 선택하세요.
    }

    // PDF 공유를 선택했을 경우
    public void OnPDF()
    {
        type = "PDF";
        StandaloneFileBrowser.OpenFilePanelAsync("Select the PDF file to share.", null, extensionList_PDF, false, SelectCallback); // 공유할 PDF 파일을 선택하세요.
    }

    // 공유할 파일을 선택하고 확인을 했을 경우
    private void SelectCallback(IList<ItemWithStream> result)
    {
        if (result != null)
        {
            string path = result[0].Name;
            
            if (!string.IsNullOrEmpty(path))
            {
                if (type == "PDF")
                {
                    // PDF일 경우 파일이름 유효성 체크 후 공유합니다.
                    string originPath = path;
                    string[] splitPath = originPath.Split("/");
                    string fileName = Regex.Replace(Path.GetFileName(originPath), "[^a-zA-Z0-9_.-]", "");
                    path = originPath.Replace(splitPath.Last(), fileName);
                    try
                    {
                        File.Copy(originPath, path);
                    }
                    catch (Exception e)
                    {
                        Request(path);
                        throw;
                    }
                }
                
                if (type == "Image")
                {
                    // 이미지일 경우 해상도 유효성 체크 후 공유합니다.
                    TextureManager.Instance.RequestCheckTexture(path, check =>
                    {
                        if (check)
                            Request(path);
                        else
                            UIManager.Instance.OpenToast(LocalizeManager.Instance.GetString("TooHighResolution"));
                    });
                    return;
                }

                Request(path);
            }
        }
    }

    // 파공유할 파일을 선택하고 확인을 했을 경우 API 요청
    private void Request(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path);
                
        List<IMultipartFormSection> form = new List<IMultipartFormSection> 
        { 
            new MultipartFormFileSection("file", data, filename, "multipart/form-data")
        };
            
        ServerManager.Instance.APIPostRequest<APINewFileUploadData>(Url.newFileUploadUrl, form, res =>
        {
            Scene.OnFile(type, NursingManager.Instance.userData.username, res.url);
            gameObject.SetActive(false);
        });
    }

    public void OnClose()
    {
        gameObject.SetActive(false);
    }
}
