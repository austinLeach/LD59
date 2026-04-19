using Unity.VisualScripting;
using UnityEngine;

public class HammerScriptHandler : MonoBehaviour
{
    public MusicBox musicBox;
    public GameObject noteBoxHammer;
    Rigidbody2D boxBody;
    TargetJoint2D targetJoint;
    Vector3 offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Load the inserted box and spawn notes
        if (musicBox.boxType == 1)
        {
            for (int i = 0; i < musicBox.noteCount; i++)
            {
                Instantiate(noteBoxHammer, new Vector3(-5 + (i * 0.4f) , i - 1f, i * 2),Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetMouseButtonDown(0));
        Vector3 mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            //TODO: NEED TO FIGURE OUT HOW TO DO MOUSE DRAG
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                targetObject.GameObject().GetComponent<TargetJoint2D>().anchor = new Vector2(mousePosition.x, mousePosition.y);
                //foundObject.
                //Physics2D.OverlapPoint(mousePosition);
                //set the anchor to current mouse
                 //= new Vector2 (mousePosition.x, mousePosition.y);
                
                //Constantly update target position to mouse position
                while (Input.GetMouseButtonDown(0) != true) {
                    targetObject.GameObject().GetComponent<TargetJoint2D>().target = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    targetObject.GameObject().GetComponent<TargetJoint2D>().enabled = true;

                }
                targetObject.GameObject().GetComponent<TargetJoint2D>().enabled = false;

            }
        }

    }
    private void FixedUpdate()
    {
        Debug.Log(Input.GetMouseButtonDown(0));
    }
}
