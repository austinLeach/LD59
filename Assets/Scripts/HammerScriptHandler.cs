using Unity.VisualScripting;
using UnityEngine;

public class HammerScriptHandler : MonoBehaviour
{
    public MusicBox musicBox;
    public GameObject noteBoxHammer;
    private GameObject foundObject;
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
        Vector3 mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject)
            {
                foundObject = transform.Find("NoteBoxHammer").gameObject;
                //foundObject.
                //Physics2D.OverlapPoint(mousePosition);
                //set the anchor to current mouse
                 //= new Vector2 (mousePosition.x, mousePosition.y);
                
                //Constantly update target position to mouse position
                while (Input.GetMouseButtonDown(0)) {                   
                    targetJoint.target = Input.mousePosition;
                    targetObject.GetComponent<TargetJoint2D>().enabled = true;

                }
                targetObject.GetComponent<TargetJoint2D>().enabled = false;
                
            }
        }

    }
}
