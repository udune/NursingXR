using System.IO;
using UnityEngine;

public class PDFShare : FileShare
{
    [SerializeField] public FileViewer_PDF fileViewer_PDF;
    
    // pdf 뷰어를 띄웁니다.
    public override void OnList()
    {
        fileViewer_PDF.gameObject.SetActive(true);
        fileViewer_PDF.SetViewer(path, file.text);
    }
}
