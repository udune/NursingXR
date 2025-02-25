using System;
using UnityEngine;

public class AED_Display : MonoBehaviour
{
    [SerializeField] public MeshRenderer renderer;
    [SerializeField] public Texture2D[] textures;

    public  float time;
    public  bool dirty0;
    public  bool dirty1;
    public  bool dirty2;
    
    private static readonly int Value = Shader.PropertyToID("_Value");
    public readonly int Texture1 = Shader.PropertyToID("_Texture1");
    public readonly int Texture2 = Shader.PropertyToID("_Texture2");

    private void Update()
    {
        var nextTime = time + Time.deltaTime;

        if (time % 1 > nextTime % 1)
            dirty0 = true;

        time = nextTime;
        renderer.material.SetFloat(Value, time);

        if (dirty0)
        {
            dirty0 = false;

            if (dirty1)
            {
                dirty1 = false;
                NextDisplay();
                return;
            }

            if (dirty2)
            {
                dirty2 = false;
                NextDisplay();
            }
        }
    }

    public void TurnOff()
    {
        renderer.enabled = false;
    }

    public void TurnOn()
    {
        time = 0;

        renderer.enabled = true;

        dirty1 = false;
        dirty2 = false;

        renderer.material.SetTexture(Texture1, textures[0]);
        renderer.material.SetTexture(Texture2, textures[1]);
    }

    public void ChangeDisp()
    {
        dirty1 = true;
        dirty2 = true;
    }

    public void NextDisplay()
    {
        if (time % 2 > 1)
            renderer.material.SetTexture(Texture1, textures[2]);
        else
            renderer.material.SetTexture(Texture2, textures[3]);
    }
}
