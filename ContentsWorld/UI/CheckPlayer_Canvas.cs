using UnityEngine;

public class CheckPlayer_Canvas : CheckPlayer
{
    protected override void AwakeAction()
    {
        base.AwakeAction();
        canvas = GetComponent<CanvasGroup>();
    }
}
