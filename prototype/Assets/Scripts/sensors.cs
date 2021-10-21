using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sensors : MonoBehaviour
{
    public LayerMask hitMask;

    public enum Type
    {
        Line,
        RayBundle,
        Spherecast,
        BoxCast
    }

    public Type sensorType = Type.Line;
    public float raycastLength = 1.0f;
    [Header("BoxExtent Settings")]
    public Vector2 boxExtents = new Vector2(1.0f, 1.0f);

    public float sphereRadius = 1.0f;

    Transform cachedTransform;


    void Start()
    {
        cachedTransform = GetComponent<Transform>();
    }

    public bool Hit
    {
        get; private set;
    }

    public RaycastHit info = new RaycastHit();

    public bool Scan()
    {
        Hit = false;
        Vector3 dir = cachedTransform.forward;
        switch (sensorType)
        {
            case Type.Line:
                if (Physics.Linecast(cachedTransform.position, cachedTransform.position + dir * raycastLength, out info, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    Debug.Log("hit");
                    return true;
                }
                break;

            case Type.RayBundle:
                break;
            case Type.Spherecast:
                break;
            case Type.BoxCast:
                if (Physics.CheckBox(this.transform.position, new Vector3(boxExtents.x, boxExtents.y, raycastLength) / 2.0f, this.transform.rotation, hitMask, QueryTriggerInteraction.Ignore))
                {
                    Hit = true;
                    return true;
                }
                break;
        }
        return false;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (cachedTransform == null)
        {
            cachedTransform = GetComponent<Transform>();
        }
        Scan();
        if (Hit) Gizmos.color = Color.red;
        Gizmos.matrix *= Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        float length = raycastLength;
        switch (sensorType)
        {
            case Type.Line:
                if (Hit) length = Vector3.Distance(this.transform.position, info.point);
                Gizmos.DrawLine(Vector3.zero, Vector3.forward * length);
                Gizmos.color = Color.green;
                Gizmos.DrawCube(Vector3.forward * length, new Vector3(0.02f, 0.02f, 0.022f));
                break;
            case Type.RayBundle:
                break;
            case Type.Spherecast:
                break;
            case Type.BoxCast:
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(boxExtents.x, boxExtents.y, length));
                break;
        }
    }
}
