using UnityEngine;

public class PullLever : MonoBehaviour
{
    public float[] maxAndMinAngle = { -90f, 0f };
    public float smoothSpeed = 0.5f;
    public RaycastHit2D hitObj;

    private Rigidbody2D rb;
    private RectTransform rectTransform;
    private Quaternion curAngle;
    private Vector2 currentMouse;
    private BoxCollider2D boxCollider;
    private float targetAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        boxCollider = GetComponent<BoxCollider2D>();
        rectTransform = GetComponent<RectTransform>();
        targetAngle = -90f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        //if left clicking and angle does not match then rotate rectTransform

        if (Input.GetMouseButton(0) && boxCollider.OverlapPoint(ray))
        {;

            hitObj = Physics2D.Raycast(ray, ray, 0);

            if (hitObj.collider != null && hitObj.collider.gameObject.CompareTag("lever"))
            {
                
            }
        }
    }

    private float GetCurrentAngle()
    {
        float z = rectTransform.localEulerAngles.z;
        // Convert to -180 to 180 range for correct angle wrapping
        return (z > 180f) ? z - 360f : z;
    }


}
