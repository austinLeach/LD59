using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform deleteTarget;
    [SerializeField] public UnityEvent onSuccess;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (deleteTarget != null && RectOverlaps(rectTransform, deleteTarget))
        {
            onSuccess.Invoke();
            Destroy(deleteTarget.gameObject);
            Destroy(gameObject);
        }
    }

    private bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Rect worldA = GetWorldRect(a);
        Rect worldB = GetWorldRect(b);
        return worldA.Overlaps(worldB);
    }

    private Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        // corners: [0]=bottom-left, [2]=top-right
        return new Rect(corners[0].x, corners[0].y,
                        corners[2].x - corners[0].x,
                        corners[2].y - corners[0].y);
    }
}
