using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForgeController : MonoBehaviour
{
    public bool pullLever = false;
    public float speed;
    public float maxDistance;

    private RectTransform rectTransform;
    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private bool moving = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = GetComponent<RectTransform>().position;

        endingPosition = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y - 4.03f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //If lever pulled and not moving
        if (moving == false && pullLever == true)
        {
            //Start moving
            if (rectTransform.position.Equals(startingPosition))
            {
                moving = true;
                rectTransform.localPosition = Vector3.Lerp(startingPosition, endingPosition, 0.2f);
                Debug.Log("Moving Down");
            }
            else if (rectTransform.position == endingPosition)
            {
                //flip
                moving = true;
                rectTransform.localPosition = Vector3.Lerp(endingPosition, startingPosition, 0.2f);
                Debug.Log("Moving Up");
            }
        }
        else if (moving == true && pullLever == true)
        {
            if (rectTransform.position.Equals(endingPosition))
            {
                moving = false;
                pullLever = false;
                Debug.Log("R E S E T!!!!!!!!!!!!");
            }
        }
    }

}
