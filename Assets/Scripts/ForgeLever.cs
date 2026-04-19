using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ForgeLever : MonoBehaviour
{
    public float[] anchorAngles = { -45f, 45f };  // Only -45° and 45° as anchor points
    public float rotationSpeed = 0.2f;
    public float smoothSpeed = 10f;

    private BoxCollider2D boxCollider;
    private RectTransform rectTransform;
    private Vector2 lastMousePosition;
    private float targetAngle;
    private bool isDragging = false;
    private float goalAngle;
    private Vector2 currentMouse;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        boxCollider = GetComponent<BoxCollider2D>();
        // Set the initial rotation to 45 degrees
        targetAngle = -45f;
        rectTransform.localEulerAngles = new Vector3(0, 0, targetAngle);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentMouse = Input.mousePosition;
        //Check if mouse is touching lever and m1 pressed down
        if (Input.GetMouseButton(0) && boxCollider.OverlapPoint(currentMouse))
            {
            float currentZ = GetCurrentAngle();

            // Smoothly rotate towards the target angle
            float smoothed = Mathf.LerpAngle(currentZ, targetAngle, Time.deltaTime * smoothSpeed);

            // Apply the smooth angle to the rotation
            rectTransform.localEulerAngles = new Vector3(0, 0, smoothed);
        }

    }

    public void SetGoalAngle()
    {
        float current = GetNearestAnchorAngle(GetCurrentAngle());

        goalAngle = (current == 45f) ? -45f : 45f;
        Debug.Log("Goal Angle: " + goalAngle);
    }

    public bool CheckWin()
    {
        float current = GetNearestAnchorAngle(GetCurrentAngle());
        if (current == goalAngle)
        {
            Debug.Log("Lever Goal Accomplished");
            goalAngle = 180;
            return true;
        }
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastMousePosition;
        lastMousePosition = eventData.position;

        // Calculate the rotation delta based on mouse movement
        float rotationDelta = -delta.y * rotationSpeed;
        targetAngle = Mathf.Clamp(GetCurrentAngle() + rotationDelta, anchorAngles[0], anchorAngles[anchorAngles.Length - 1]);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        targetAngle = GetNearestAnchorAngle(GetCurrentAngle());
    }

    private float GetCurrentAngle()
    {
        float z = rectTransform.localEulerAngles.z;
        // Convert to -180 to 180 range for correct angle wrapping
        return (z > 180f) ? z - 360f : z;
    }

    private float GetNearestAnchorAngle(float angle)
    {
        // Get the nearest anchor angle from the list
        float closest = anchorAngles[0];
        float minDiff = Mathf.Abs(angle - closest);

        foreach (float anchor in anchorAngles)
        {
            float diff = Mathf.Abs(angle - anchor);
            if (diff < minDiff)
            {
                minDiff = diff;
                closest = anchor;
            }
        }

        if (closest == 0)
        {
            if (angle < 0)
            {
                closest = -45;
            }
            else
            {
                closest = 45;
            }
        }
        return closest;
    }
}
