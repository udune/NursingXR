using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FileViewer_Image : FileViewer
{
    [Header("Image")]
    [SerializeField] FileViewer_PDF pdf;
    [SerializeField] RawImage viewer;
    private List<FileItem_Image> itemList = new ();

    private Vector2 size;

    public override void Awake()
    {
        base.Awake();
        size = viewer.GetComponent<RectTransform>().sizeDelta;
    }

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
    
    // 메인 뷰어에 이미지를 출력합니다.
    public override void SetViewer(string filePath, string fileName)
    {
        base.SetViewer(filePath, fileName);
        TextureManager.Instance.RequestTexture(filePath, texture =>
        {
            Utility.SetTextureRatio(viewer, texture, size);
        });
    }

    // 뷰어 오른쪽에 아이템 리스트들을 불러옵니다.
    protected override void SetList()
    {
        for (int i = 0; i < Scene.data.File_Data.File_Items_Image.Count; i++)
        {
            if (itemList.Count > 0 && itemList.Count > i)
            {
                itemList[i].SetViewer(Scene.data.File_Data.File_Items_Image[i].image, Path.GetFileName(Scene.data.File_Data.File_Items_Image[i].image));
                continue;
            }
            
            GameObject itemGo = Instantiate(item, contents);
            FileItem_Image fileItem = itemGo.GetComponent<FileItem_Image>();
            itemGo.SetActive(true);
            fileItem.SetViewer(Scene.data.File_Data.File_Items_Image[i].image, Path.GetFileName(Scene.data.File_Data.File_Items_Image[i].image));
            itemList.Add(fileItem);
        }
    }
}
