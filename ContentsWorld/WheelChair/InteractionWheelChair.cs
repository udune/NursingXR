using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InteractionWheelChair : MonoBehaviour
{
    [SerializeField] public WheelChair wheelChair;
    [SerializeField] public Outline[] outlines;

    public virtual void OnPointerDown()
    {
    }

    public virtual void OnPointerEnter()
    {
        if (outlines == null) return;
        foreach (Outline outline in outlines)
        {
            outline.enabled = true;
            DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 3.0f, 0.25f);
        }
    }

    public virtual void OnPointerExit()
    {
        if (outlines == null) return;
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
            DOTween.To(() => outline.OutlineWidth, x => outline.OutlineWidth = x, 0.0f, 0.25f);
        }
    }
}
