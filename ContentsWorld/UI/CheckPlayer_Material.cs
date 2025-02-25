using UnityEngine;

public class CheckPlayer_Material : CheckPlayer
{
    protected override void AwakeAction()
    {
        base.AwakeAction();
        mat = GetComponent<MeshRenderer>().material;
    }
}
