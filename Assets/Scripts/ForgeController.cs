using JetBrains.Annotations;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForgeController : MonoBehaviour
{
    public bool pullLever = false;
    public float speed;
    public float maxDistance;
    public float moveTime = 10f;
    public bool onOff = false;

    private RectTransform rectTransform;
    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private bool moving = false;
    private GameObject[] items;
    private Collider2D[] colliderArray;
    private PolygonCollider2D polyCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        startingPosition = GetComponent<RectTransform>().position;

        endingPosition = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y - 4.03f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        //If lever pulled and not moving
        if (moving == false && pullLever == true)
        {
            //Start moving
            if (GetComponent<RectTransform>().position == startingPosition)
            {
                moving = true;
                onOff = !onOff;
                GetComponent<RectTransform>().position = Vector3.Lerp(startingPosition, endingPosition, moveTime);
                GlobalVariables.Timer(ref moving, ref moveTime);

                //Debug.Log("Moving Down");
            }
            else if (GetComponent<RectTransform>().position == endingPosition)
            {
                //Move up and check boxes

                moving = true;
                onOff = !onOff;

                GetComponent<RectTransform>().position = Vector3.Lerp(endingPosition, startingPosition, moveTime);
                    GlobalVariables.Timer(ref moving, ref moveTime);
                    //Debug.Log("Moving Up");

                moving = false;
                pullLever = false;
            }
        }
        else if (moving == true && pullLever == true)
        {
            if (GetComponent<RectTransform>().position == endingPosition)
            {
                moving = false;
                pullLever = false;
                //Debug.Log("R E S E T!!!!!!!!!!!!");
            }
        }
    }

}
