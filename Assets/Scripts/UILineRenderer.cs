using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRenderer : Graphic
{
    [SerializeField] public float lineWidth = 8f;

    private Vector2[] points = new Vector2[0];

    public void SetPoints(Vector2[] newPoints)
    {
        points = newPoints;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (points == null || points.Length < 2) return;

        for (int i = 0; i < points.Length - 1; i++)
        {
            Vector2 a = points[i];
            Vector2 b = points[i + 1];

            Vector2 dir = (b - a).normalized;
            Vector2 perp = new Vector2(-dir.y, dir.x) * (lineWidth * 0.5f);

            int idx = i * 4;
            UIVertex v = new UIVertex();
            v.color = color;

            v.position = a + perp; vh.AddVert(v);
            v.position = a - perp; vh.AddVert(v);
            v.position = b - perp; vh.AddVert(v);
            v.position = b + perp; vh.AddVert(v);

            vh.AddTriangle(idx,     idx + 1, idx + 2);
            vh.AddTriangle(idx,     idx + 2, idx + 3);
        }
    }
}
