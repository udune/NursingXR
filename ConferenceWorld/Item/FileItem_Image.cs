using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FileItem_Image : FileItem
{
    [SerializeField] FileViewer_Image fileViewer;
    [SerializeField] RawImage viewer;

    private Vector2 size;

    private float ratiox;
    private float ratioy;
    
    private float constSize = 136.0f;
    
    private void Awake()
    {
        size = viewer.GetComponent<RectTransform>().sizeDelta;
    }

    // 이미지 뷰어 오른쪽 아이템 목록들에 이미지를 출력합니다.
    public override void SetViewer(string filePath, string fileName)
    {
        base.SetViewer(filePath, fileName);
        TextureManager.Instance.RequestTexture(filePath, texture =>
        {
            ratiox = (float) texture.width / texture.height;
            ratioy = (float) texture.height / texture.width;
            
            float x = constSize * ratiox;
            float y = constSize * ratioy;
            if (x >= constSize)
            {
                x = constSize;
                y *= (x / constSize);
            }
            if (y >= constSize)
            {
                y = constSize;
                x *= (y / constSize);
            }
            
            viewer.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);
            viewer.texture = texture;
        });
    }

    // 아이템을 클릭했을때 해당 아이템 이미지가 뷰어에 표시됩니다.
    public override void OnViewer()
    {
        fileViewer.SetViewer(filePath, fileName);
    }
}
