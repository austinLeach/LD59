using System.Drawing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HammerMoveScript : MonoBehaviour
{
    Rigidbody2D rigidBody2D;
    public Point pivot;
    public float[] anchorAngles = { -45f, 45f };  // Only -45° and 45° as anchor points
    private RectTransform rectTransform;
    private Vector2 lastMousePosition;
    private float goalAngle;
    private float targetAngle;
    private bool isDragging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
