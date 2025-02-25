using UnityEngine;

public class RopeNode : MonoBehaviour
{
    [SerializeField] Rope parent;
    [SerializeField] bool ignore;

    private RopeNode prevNode;
    private RopeNode nextNode;
    public Vector3 Velocity { get; private set; }

    public void Init(RopeNode prev, RopeNode next)
    {
        prevNode = prev;
        nextNode = next;

        InitLayer();
    }

    private void Reset()
    {
        parent = GetComponentInParent<Rope>();
        if (parent == null)
            Destroy(this);
    }

    #region Velocity

    public void ResetVelocity()
    {
        Velocity = Vector3.zero;
    }

    public void CalcGravity()
    {
        if (ignore)
            return;

        Velocity += new Vector3(0, -9.8f, 0) * Time.fixedDeltaTime;
    }

    public void CalcTension()
    {
        if (ignore)
            return;

        Vector3 velocity = Vector3.zero;
        
        if (prevNode != null)
            velocity += CalcTension(prevNode);
        
        if (nextNode != null)
            velocity += CalcTension(nextNode);
        
        Velocity += velocity;
    }

    private Vector3 CalcTension(RopeNode node)
    {
        var currentposition = transform.position + Velocity;
        var otherposition = node.transform.position + node.Velocity;
        var delta = otherposition - currentposition;
        var distance = delta.magnitude;
        var direction = delta.normalized;

        return (distance - parent.Width) * 0.5f * direction;
    }

    public bool ApplyVelocity()
    {
        if (ignore)
            return true;

        if (Velocity == Vector3.zero)
            return false;

        if (RayCast())
        {
            transform.position = raycastResult[0].point + raycastResult[0].normal * parent.SizeAndOffset;
            return true;
        }
        transform.position += Velocity;
        return true;
    }
    #endregion

    #region RayCast
    private const int RAYCAST_MAXCOUNT = 1;

    private RaycastHit[] raycastResult = new RaycastHit[RAYCAST_MAXCOUNT];
    private int raycastLayer;

    private void InitLayer()
    {
        raycastLayer = 1 << gameObject.layer | 1 << 6 | 1 << 8;
    }

    private bool RayCast()
    {
        if (SphereCast() > 0)
        {
            if (raycastResult[0].distance > 0)
                return true;
        }
        return false;
    }

    private int SphereCast()
    {
        return Physics.SphereCastNonAlloc(transform.position, 0.01f, Velocity.normalized, raycastResult, Velocity.magnitude + 0.005f, ~raycastLayer);
    }
    #endregion
}
