using UnityEngine;
using UnityEngine.UI;

public class ImageShare : FileShare
{
    [SerializeField] public FileViewer_Image fileViewer_Image;
    [SerializeField] RawImage viewer;
    private Vector2 size;
    private RectTransform rect;
    private float constSize = 245.0f;
    
    private void Awake()
    {
        rect = viewer.GetComponent<RectTransform>();
        size = rect.sizeDelta;
    }

    // 썸네일에 공유한 이미지를 출력합니다.
    public override void SetData(string username, string file, string text = null)
    {
        path = file;
        this.username.text = username;
        TextureManager.Instance.RequestTexture(path, texture =>
        {
            float ratiox = (float) texture.width / texture.height;
            float ratioy = (float) texture.height / texture.width;
            
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
    
    // 뷰어 및 아이템리스트를 띄웁니다.
    public override void OnList()
    {
        fileViewer_Image.gameObject.SetActive(true);
        fileViewer_Image.SetViewer(path, file.text);
    }
}
