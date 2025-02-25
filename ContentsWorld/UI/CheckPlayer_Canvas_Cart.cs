
public class CheckPlayer_Canvas_Cart : CheckPlayer_Canvas
{
    protected override void FadeIn()
    {
        base.FadeIn();
        contentsWorldUI.cartInventoryUI.EnterCartRange();
    }
    
    protected override void FadeOut()
    {
        base.FadeOut();
        contentsWorldUI.cartInventoryUI.ExitCartRange();
    }
}
