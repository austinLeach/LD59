using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ForgeController : MonoBehaviour
{
    public float speed;
    public float maxDistance;
    private RectTransform rectTransform;
    private Vector3 lastMousePosition;
    private Vector3 startingPosition;
    private Vector3 endingPosition;
    public GameObject Forge;
    private bool moving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = new Vector3(-4, 5, 0);

        endingPosition = new Vector3(-4, 10, 0);
    }

    // Update is called once per frame
    void Update()
    {

        Forge.transform.Translate(Vector3.down * (speed * timeDelta);


    }

    public void OnPointerDown(PointerEventData eventData)
    {


    }
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");

    }
}
