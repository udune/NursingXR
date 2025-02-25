using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FileItem_PDF : FileItem
{
    [SerializeField] FileViewer_PDF fileViewer;
    [SerializeField] TMP_Text name;
    
    // pdf 뷰어 오른쪽 아이템 목록들에 이름을 설정합니다.
    public override void SetViewer(string filePath, string fileName)
    {
        base.SetViewer(filePath, fileName);
        name.text = fileName;
    }

    // 아이템을 클릭했을때 해당 아이템 pdf가 뷰어에 출력됩니다.
    public override void OnViewer()
    {
        fileViewer.SetViewer(filePath, fileName);
    }
}
