using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DebugDrawBox : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null)
            boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
            return;

        // Set the Gizmo color
        Gizmos.color = Color.blue;

        // Get the center and size of the BoxCollider in world space
        Vector3 center = transform.TransformPoint(boxCollider.center);
        Vector3 size = Vector3.Scale(boxCollider.size, transform.lossyScale);

        // Calculate the 8 corners of the box
        Vector3 halfSize = size / 2f;

        Vector3[] corners = new Vector3[8]
        {
            center + transform.TransformDirection(new Vector3(-halfSize.x, -halfSize.y, -halfSize.z)),
            center + transform.TransformDirection(new Vector3(halfSize.x, -halfSize.y, -halfSize.z)),
            center + transform.TransformDirection(new Vector3(halfSize.x, -halfSize.y, halfSize.z)),
            center + transform.TransformDirection(new Vector3(-halfSize.x, -halfSize.y, halfSize.z)),
            center + transform.TransformDirection(new Vector3(-halfSize.x, halfSize.y, -halfSize.z)),
            center + transform.TransformDirection(new Vector3(halfSize.x, halfSize.y, -halfSize.z)),
            center + transform.TransformDirection(new Vector3(halfSize.x, halfSize.y, halfSize.z)),
            center + transform.TransformDirection(new Vector3(-halfSize.x, halfSize.y, halfSize.z))
        };

        // Draw the edges of the box
        // Bottom face
        Gizmos.DrawLine(corners[0], corners[1]);
        Gizmos.DrawLine(corners[1], corners[2]);
        Gizmos.DrawLine(corners[2], corners[3]);
        Gizmos.DrawLine(corners[3], corners[0]);

        // Top face
        Gizmos.DrawLine(corners[4], corners[5]);
        Gizmos.DrawLine(corners[5], corners[6]);
        Gizmos.DrawLine(corners[6], corners[7]);
        Gizmos.DrawLine(corners[7], corners[4]);

        // Vertical edges
        Gizmos.DrawLine(corners[0], corners[4]);
        Gizmos.DrawLine(corners[1], corners[5]);
        Gizmos.DrawLine(corners[2], corners[6]);
        Gizmos.DrawLine(corners[3], corners[7]);
    }
}
