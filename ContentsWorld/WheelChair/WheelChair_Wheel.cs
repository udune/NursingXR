using UnityEngine;

public class WheelChair_Wheel : MonoBehaviour
{
    [SerializeField] Transform trn1;
    [SerializeField] Transform trn2;
    [SerializeField] Transform trn3;
    [SerializeField] Transform trn4;
    private float angle;
    public bool IsRotate;

    void Start()
    {
        IsRotate = false;
    }

    private void Update()
    {
        if (!IsRotate)
            return;

        if (angle > 360)
            angle -= 360;
		
        angle += Time.deltaTime * 180;

        var quaternion = Quaternion.Euler(angle, 0, 0);
        trn1.localRotation = quaternion;
        trn2.localRotation = quaternion;
        trn3.localRotation = quaternion;
        trn4.localRotation = quaternion;
    }
}
