using UnityEngine;

public class Suction_Needle : MonoBehaviour
{
    [SerializeField] Suction_Button suctionBtn;

    private float value;
    private float valueRange;
    private float targetRange;

    private void Update()
    {
        if (!Mathf.Approximately(valueRange, targetRange))
        {
            valueRange = Mathf.MoveTowards(valueRange, targetRange, Time.deltaTime * 5);

            var range = EaseInOutCubic(valueRange, 1);
            float target;
            switch (suctionBtn.Power)
            {
                default:
                    target = 110 + 20 * range;
                    break;
                case 1:
                    target = 30 + 35 * range;
                    break;
                case 2:
                    target = -55 + 85 * range;
                    break;
            }
            value = Mathf.MoveTowards(value, target, Time.deltaTime * 45);
            transform.localRotation = Quaternion.Euler(0, value, 0);
        }
        else
            targetRange = targetRange == 0 ? 1 : 0;
    }

    private float EaseInOutCubic(float time, float duration)
    {
        if ((time /= duration * 0.5f) < 1)
            return 0.5f * time * time * time;
        return 0.5f * ((time -= 2) * time * time + 2);
    }
}
