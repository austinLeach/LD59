using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpinUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private int requiredRotations = 3;
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] public UnityEvent onSuccess;

    private RectTransform rectTransform;
    private bool isDragging;
    private float totalAngle;
    private float lastAngle;
    private bool completed;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        lastAngle = GetAngle(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || completed) return;

        float currentAngle = GetAngle(eventData.position);
        float delta = Mathf.DeltaAngle(lastAngle, currentAngle);
        totalAngle += delta;
        lastAngle = currentAngle;

        // Rotate the element visually
        rectTransform.localEulerAngles = new Vector3(0f, 0f, rectTransform.localEulerAngles.z + delta);

        if (Mathf.Abs(totalAngle) >= requiredRotations * 360f)
        {
            completed = true;
            if (objectToDestroy != null) Destroy(objectToDestroy);
            onSuccess.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void ResetSpin()
    {
        totalAngle = 0f;
        lastAngle = 0f;
        isDragging = false;
        completed = false;
        rectTransform.localEulerAngles = Vector3.zero;
    }

    // Returns the angle in degrees from the element's center to the pointer
    private float GetAngle(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, screenPos, null, out Vector2 localPos);
        return Mathf.Atan2(localPos.y, localPos.x) * Mathf.Rad2Deg;
    }
}
