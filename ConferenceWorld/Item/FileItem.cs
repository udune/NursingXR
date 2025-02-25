using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class FileItem : MonoBehaviour
{
    protected string filePath;
    protected string fileName;
    
    // filePath, fileName을 저장합니다.
    public virtual void SetViewer(string filePath, string fileName)
    {
        this.filePath = filePath;
        this.fileName = fileName;
    }

    public abstract void OnViewer();
}
