using System;
using TMPro;
using UnityEngine;

public class Suction_Water : MonoBehaviour
{
    [SerializeField] Suction_Button suctionButton;
    [SerializeField] TMP_Text suction_Text;

    private float value;
    private float fade;

    public bool Power;
    private SkinnedMeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();
    }

    public void ResetHeight()
    {
        Power = false;
        value = 0;
        renderer.SetBlendShapeWeight(0, 0);
    }

    private void Update()
    {
        var value = Mathf.Clamp(this.value + Time.deltaTime * (Power ? suctionButton.Power * 0.5f : 0), 0, 100);

        if (Power && !Mathf.Approximately(this.value, value))
        {
            this.value = value;
            renderer.SetBlendShapeWeight(0, this.value);
            fade = Mathf.Clamp(fade + Time.deltaTime * 2, 0, 5);
            suction_Text.text = $"{this.value:N0}ml";
            suction_Text.color = new Color(1, 1, 1, fade);
            suction_Text.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -0.1f + this.value * 0.01f * 0.245f);
        }
        else
        {
            fade = Mathf.Clamp(fade - Time.deltaTime * 2, 0, 5);
            suction_Text.color = new Color(1, 1, 1, fade);
        }
    }
}
