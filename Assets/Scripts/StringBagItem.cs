using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StringBagItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GuitarString ropePrefab;
    [SerializeField] private RectTransform guitarTarget;
    [SerializeField] private GameObject[] elementsToActivate;
    [SerializeField] public UnityEvent onSuccess;
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private Color ropeColor = Color.white;
    [SerializeField] private float ropeLineWidth = 8f;

    private GuitarString activeRope;
    private UILineRenderer activeRopeLine;

    private Vector2 PointerToWorld(Vector2 screenPos)
    {
        float dist = Mathf.Abs(Camera.main.transform.position.z);
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Create the UI line renderer as a child of the canvas so it draws on top
        GameObject lineGO = new GameObject("_RopeLine");
        lineGO.transform.SetParent(targetCanvas.transform, false);
        activeRopeLine = lineGO.AddComponent<UILineRenderer>();
        activeRopeLine.color = ropeColor;
        activeRopeLine.lineWidth = ropeLineWidth;

        // Stretch the RectTransform to fill the canvas so local positions work correctly
        RectTransform lineRect = lineGO.GetComponent<RectTransform>();
        lineRect.anchorMin = Vector2.zero;
        lineRect.anchorMax = Vector2.one;
        lineRect.offsetMin = Vector2.zero;
        lineRect.offsetMax = Vector2.zero;

        Camera cam = targetCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : targetCanvas.worldCamera;

        activeRope = Instantiate(ropePrefab);
        activeRope.Build(PointerToWorld(eventData.position), activeRopeLine, targetCanvas.GetComponent<RectTransform>(), cam);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (activeRope != null)
            activeRope.SetAnchorPosition(PointerToWorld(eventData.position));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (activeRope == null) return;

        bool droppedOnGuitar = RectTransformUtility.RectangleContainsScreenPoint(
            guitarTarget, eventData.position, eventData.pressEventCamera);

        if (droppedOnGuitar)
        {
            Destroy(activeRopeLine.gameObject);
            Destroy(activeRope.gameObject);

            foreach (GameObject elem in elementsToActivate)
                if (elem != null) elem.SetActive(true);

            onSuccess.Invoke();
        }
        else
        {
            Destroy(activeRopeLine.gameObject);
            Destroy(activeRope.gameObject);
        }

        activeRope = null;
        activeRopeLine = null;
    }
}
