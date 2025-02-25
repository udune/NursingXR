using DG.Tweening;
using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TMP_Text info_Text;
    private CanvasGroup canvas;
    
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0.0f;
    }
    
    public void SetEnable(string value)
    {
        info_Text.text = value;
        
        if (canvas.alpha > 0.0f)
            return;
        canvas.DOFade(1, 0.25f);
    }

    public void SetDisable()
    {
        canvas.DOFade(0, 0.25f);
    }
}
