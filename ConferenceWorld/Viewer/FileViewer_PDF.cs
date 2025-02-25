using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Paroxe.PdfRenderer;
using UnityEngine;
using Vuplex.WebView;

public class FileViewer_PDF : FileViewer
{
    [Header("PDF")]
    [SerializeField] FileViewer_Image image;
    [SerializeField] PDFViewer pdfViewer;
    private List<FileItem_PDF> itemList = new ();

    // enable: input, 움직임을 비활성화합니다.
    public override void OnEnable()
    {
        base.OnEnable();
        InputManager.Instance.isLock = true;
        NursingManager.Instance.character.SetMoveState(false);
    }

    // disable: input, 움직임을 활성화합니다.
    public override void OnDisable()
    {
        base.OnDisable();
        InputManager.Instance.isLock = false;
        NursingManager.Instance.character.SetMoveState(true);
    }
    
    // 메인 뷰어에 pdf를 출력합니다.
    public override void SetViewer(string filePath, string fileName)
    {
        base.SetViewer(filePath, fileName);
        pdfViewer.FileURL = filePath;
        pdfViewer.gameObject.SetActive(false);
        pdfViewer.gameObject.SetActive(true);
    }
    
    // 뷰어 오른쪽에 아이템 리스트들을 불러옵니다.
    protected override void SetList()
    {
        for (int i = 0; i < Scene.data.File_Data.File_Items_PDF.Count; i++)
        {
            if (itemList.Count > 0 && itemList.Count > i)
            {
                itemList[i].SetViewer(Scene.data.File_Data.File_Items_PDF[i].pdf, Path.GetFileName(Scene.data.File_Data.File_Items_PDF[i].pdf));
                continue;
            }
            
            GameObject itemGo = Instantiate(item, contents);
            FileItem_PDF fileItem = itemGo.GetComponent<FileItem_PDF>();
            fileItem.SetViewer(Scene.data.File_Data.File_Items_PDF[i].pdf, Path.GetFileName(Scene.data.File_Data.File_Items_PDF[i].pdf));
            itemGo.SetActive(true);
            itemList.Add(fileItem);
        }
    }
}
