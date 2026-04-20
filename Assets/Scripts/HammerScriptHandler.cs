using Unity.VisualScripting;
using UnityEngine;

public class HammerScriptHandler : MonoBehaviour
{
    public MusicBox musicBox;
    public GameObject noteBoxHammer;
    public RaycastHit2D hitObj;

    private Rigidbody2D boxBody;
    private TargetJoint2D targetJoint;
    private Vector3 offset;
    private bool dragging;
    private bool anchorSet = false;
    private string tagName = "Box";

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

        FindFirstObjectByType<HammerCompletionZone>()?.StartMinigame();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 ray = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
       
        dragging = Input.GetMouseButton(0);
        //Debug.Log(dragging);


        if (dragging == true)
        {
            int layerObject = 3;
            if (anchorSet == false)
            {
                
                hitObj = Physics2D.Raycast(ray, ray, layerObject);
            }
            if (hitObj.collider != null && hitObj.collider.gameObject.CompareTag("Box"))
            {
                //Debug.Log(hitObj.collider.gameObject.GetComponent<TargetJoint2D>().anchor);
                //set anchor point
                if (anchorSet == false)
                {
                    targetJoint = hitObj.collider.gameObject.GetComponent<TargetJoint2D>();
                    hitObj.collider.gameObject.GetComponent<TargetJoint2D>().anchor = ray.normalized * 0.1f;
                    hitObj.collider.gameObject.GetComponent<TargetJoint2D>().target = new Vector2(ray.x, ray.y);
                    anchorSet = true;
                }
                //Constantly update target position to mouse position


            }

            //Constantly move the box to cursor
            if (hitObj.collider.gameObject.CompareTag("Box"))
            {
                hitObj.collider.gameObject.GetComponent<TargetJoint2D>().target = new Vector2(ray.x, ray.y);
            }
        }
        else
        {
            if (anchorSet == true)
                anchorSet = false;
        }

    }

}
