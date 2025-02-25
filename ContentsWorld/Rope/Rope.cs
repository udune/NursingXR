using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField] GameObject renderer;
    [SerializeField] RopeNode[] nodes;
    public Transform startNode;
    public Transform endNode;
    private int count;
    private float inverseSize;

    [HideInInspector] public float Width;
    public float SizeAndOffset { get; private set; }

    private bool IsInitialized;

    private void Start()
    {
        Initialize();
    }

    public void SetActive_Renderer(GameObject holder, bool isOn)
    {
        transform.position = holder.transform.position;
        renderer.SetActive(isOn);
    }

    public void ResetRopePosition()
    {
        Initialize();

        var position = (startNode.position + endNode.position) / 2;

        for (int i = 1; i < count - 1; i++)
        {
            nodes[i].transform.position = position;
        }

        for (int j = 0; j < 15; j++)
        {
            for (int i = 1; i < count - 1; i++)
                nodes[i].CalcTension();
        }
        LateUpdate();
    }

    private void Initialize()
    {
        if (IsInitialized)
            return;
        
        IsInitialized = true;

        count = nodes.Length;
        SizeAndOffset = 0.01f + 0.005f;
        inverseSize = 1 / 0.01f;

        startNode = nodes[0].transform;
        endNode = nodes[count - 1].transform;

        nodes[0].Init(null, nodes[1]);
        for (int i = 1; i < count - 1; i++)
            nodes[i].Init(nodes[i - 1], nodes[i + 1]);
        nodes[count - 1].Init(nodes[count - 2], null);
    }

    private void LateUpdate()
    {
        SizeAndOffset = 0.01f + 0.005f;

        for (int i = 1; i < count - 1; i++)
        {
            nodes[i].ApplyVelocity();
            nodes[i].ResetVelocity();
        }

        UpdateLength();
    }

    private void FixedUpdate()
    {
        for (int i = 1; i < count - 1; i++)
        {
            nodes[i].CalcGravity();
            nodes[i].CalcTension();
        }
    }

    private void UpdateLength()
    {
        var distance = (startNode.position - endNode.position).magnitude * inverseSize;
        if (distance > 1)
            Width = Mathf.Log(distance, inverseSize) / nodes.Length * 0.01f * 10.0f;
        else
            Width = 0;
    }
}
