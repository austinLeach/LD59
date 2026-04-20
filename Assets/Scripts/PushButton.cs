using Unity.VisualScripting;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    public GameObject forge;
    public bool pushOnOff = false;

    private BoxCollider2D boxCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Grab mouse position
        Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (Input.GetMouseButtonDown(0))
        {

            if (boxCollider.OverlapPoint(ray))
            {
                pushOnOff = !pushOnOff;
                //activate pull lever in forge controller
                forge.gameObject.GetComponent<ForgeController>().pullLever = true;
                forge.gameObject.GetComponent<ForgeController>().onOff = true;
                //Debug.Log("Overlap: " + forge.gameObject.GetComponent<ForgeController>().pullLever);

            }
        }

    }
}
