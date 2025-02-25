using UnityEngine;
using UnityEngine.Serialization;

public class CartIcon : MonoBehaviour
{
    [SerializeField] Interaction_Item cartObject;
    private float time;
    public string Title;

    private void OnEnable()
    {
        time = 0;
    }

    private void Update()
    {
        if (time < 1)
        {
            time = Mathf.MoveTowards(time, 1, Time.deltaTime * 2);
            var value = Bounce(time, 4, 0.25f);
            transform.localScale = Vector3.one * value;
        }
    }

    private float Bounce(float value, int bounces, float bounciness)
    {
        float v1 = Mathf.Max(float.Epsilon, Mathf.Pow(bounciness, bounces) * value - value + 1);
        float v2 = Mathf.Pow(bounciness, Mathf.FloorToInt(Mathf.Log(v1, bounciness)));

        return 4 * (v1 - v2) * (v2 * bounciness - v1) / ((1 - bounciness) * (1 - bounciness)) + value;
    }

    public bool GetIconActive()
    {
        return gameObject.activeSelf;
    }

    // 카트 위에 올려진 물품을 활성화/비활성화 합니다.
    public void SetCartObjectActive(bool value)
    {
        if (cartObject.IsItem_Mount)
            return;
        cartObject.gameObject.SetActive(value);
    }

    public float GetPositionX()
    {
        return GetComponent<RectTransform>().anchoredPosition.x;
    }
}
