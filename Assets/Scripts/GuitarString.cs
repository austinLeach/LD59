using UnityEngine;

public class GuitarString : MonoBehaviour
{
    [Header("Rope")]
    public int segmentCount = 20;
    public float segmentLength = 0.15f;

    private Rigidbody2D anchorBody;
    private Rigidbody2D[] segments;

    private UILineRenderer uiLine;
    private RectTransform canvasRect;
    private Camera uiCamera;
    private Camera physicsCamera;

    public void Build(Vector2 worldPos, UILineRenderer line, RectTransform canvas, Camera cam, Camera physicsCam = null)
    {
        uiLine = line;
        canvasRect = canvas;
        uiCamera = cam;
        physicsCamera = physicsCam != null ? physicsCam : Camera.main;

        // Kinematic anchor — follows the pointer
        GameObject anchorGO = new GameObject("_Anchor");
        anchorGO.transform.SetParent(transform);
        anchorBody = anchorGO.AddComponent<Rigidbody2D>();
        anchorBody.bodyType = RigidbodyType2D.Kinematic;
        anchorBody.position = worldPos;

        segments = new Rigidbody2D[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segGO = new GameObject("_Seg" + i);
            segGO.transform.SetParent(transform);

            Rigidbody2D rb = segGO.AddComponent<Rigidbody2D>();
            rb.mass = 0.1f;
            rb.gravityScale = 1f;
            rb.linearDamping = 2f;
            rb.angularDamping = 2f;
            rb.position = worldPos + Vector2.down * (i + 1) * segmentLength;

            DistanceJoint2D joint = segGO.AddComponent<DistanceJoint2D>();
            joint.connectedBody = i == 0 ? anchorBody : segments[i - 1];
            joint.distance = segmentLength;
            joint.maxDistanceOnly = true;
            joint.autoConfigureDistance = false;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector2.zero;
            joint.connectedAnchor = Vector2.zero;

            segments[i] = rb;
        }
    }

    public void SetAnchorPosition(Vector2 worldPos)
    {
        anchorBody.MovePosition(worldPos);
    }

    // Stops the rope from following the pointer — it hangs in place
    public void LockInPlace()
    {
        anchorBody.bodyType = RigidbodyType2D.Static;
    }

    private void Update()
    {
        if (uiLine == null || segments == null) return;

        Vector2[] localPoints = new Vector2[segmentCount + 1];
        localPoints[0] = WorldToCanvasLocal(anchorBody.position);
        for (int i = 0; i < segments.Length; i++)
            localPoints[i + 1] = WorldToCanvasLocal(segments[i].position);

        uiLine.SetPoints(localPoints);
    }

    private Vector2 WorldToCanvasLocal(Vector2 worldPos)
    {
        Vector2 screenPos = physicsCamera.WorldToScreenPoint(worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out Vector2 localPos);
        return localPos;
    }
}
