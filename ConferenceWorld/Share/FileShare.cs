using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FileShare : MonoBehaviour
{
    [SerializeField] protected TMP_Text username;
    [SerializeField] protected TMP_Text file;
    protected string path;
    
    // username, path, filename을 저장합니다.
    public virtual void SetData(string username, string file, string text = null)
    {
        path = file;
        this.username.text = username;
        this.file.text = !string.IsNullOrEmpty(text) ? text : Path.GetFileName(file);
    }
    
    public virtual void OnList() { }
}
