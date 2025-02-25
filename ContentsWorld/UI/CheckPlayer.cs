using DG.Tweening;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    protected ContentsWorldUI contentsWorldUI;
    protected CanvasGroup canvas;
    protected Material mat;

    private ContentsWorldScene Scene;

    [SerializeField] float checkRadius = 6.0f;

    private void Awake()
    {
        AwakeAction();
    }

    protected virtual void AwakeAction()
    {
        contentsWorldUI = FindObjectOfType<ContentsWorldUI>();
        Scene = FindObjectOfType<ContentsWorldScene>();
    }

    private void Update()
    {
        if (Scene.character == null)
            return;

        if ((Scene.character.transform.position - transform.position).sqrMagnitude < checkRadius)
            FadeIn();
        else
            FadeOut();
    }

    protected virtual void FadeIn()
    {
        if (canvas != null)
            canvas.DOFade(1, 0.5f);
        if (mat != null)
            mat.DOFade(1, 0.5f);
    }
    
    protected virtual void FadeOut()
    {
        if (canvas != null)
            canvas.DOFade(0, 0.5f);
        if (mat != null)
            mat.DOFade(0, 0.5f);
    }
}
