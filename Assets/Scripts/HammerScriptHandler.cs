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
    [SerializeField] private string colliderLayerToDisable = "thingamadoob";
    private Collider2D[] disabledColliders;
    [SerializeField] private GameObject objectToPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (objectToPosition != null)
        // {
        //     objectToPosition.transform.position = new Vector3(1.41f, 0.32f, objectToPosition.transform.position.z);
        // }
        //Load the inserted box and spawn notes
        if (musicBox.boxType == 1)
        {
            DisableInterfering();
            for (int i = 0; i < musicBox.noteCount; i++)
            {
                Instantiate(noteBoxHammer, new Vector3(-5 + (i * 0.4f) , i - 1f, i * 2),Quaternion.identity);
            }
        }

        FindFirstObjectByType<HammerCompletionZone>()?.StartMinigame();
        
    }

    private void DisableInterfering()
    {
        int layer = LayerMask.NameToLayer(colliderLayerToDisable);
        if (layer == -1) return;
        int buttonLayer = LayerMask.NameToLayer("button");
        disabledColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        System.Collections.Generic.List<Collider2D> disabled = new System.Collections.Generic.List<Collider2D>();
        foreach (Collider2D col in disabledColliders)
        {
            if (col.gameObject.layer == layer && col.gameObject.layer != buttonLayer)
            {
                col.enabled = false;
                disabled.Add(col);
            }
        }
        disabledColliders = disabled.ToArray();
    }

    public void EnableInterfering()
    {
        Debug.Log("[HammerScriptHandler] EnableInterfering called.");
        if (disabledColliders != null)
        {
            foreach (Collider2D col in disabledColliders)
                if (col != null) col.enabled = true;
            disabledColliders = null;
        }
        else
        {
            // Fallback: re-find by layer in case the array was lost
            int layer = LayerMask.NameToLayer(colliderLayerToDisable);
            if (layer == -1) { Debug.LogWarning("[HammerScriptHandler] Layer not found: " + colliderLayerToDisable); return; }
            foreach (Collider2D col in FindObjectsByType<Collider2D>(FindObjectsSortMode.None))
                if (col.gameObject.layer == layer) col.enabled = true;
            Debug.Log("[HammerScriptHandler] Re-enabled via fallback.");
        }
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
                ContactFilter2D filter = new ContactFilter2D();
                filter.useTriggers = false;
                filter.useLayerMask = false;
                Collider2D[] results = new Collider2D[5];
                int count = Physics2D.OverlapPoint(ray, filter, results);
                hitObj = default;
                for (int i = 0; i < count; i++)
                {
                    if (results[i].CompareTag("Box"))
                    {
                        hitObj = new RaycastHit2D();
                        // store via a workaround — use the collider directly
                        results[i].gameObject.GetComponent<TargetJoint2D>().anchor = ray.normalized * 0.1f;
                        results[i].gameObject.GetComponent<TargetJoint2D>().target = ray;
                        targetJoint = results[i].gameObject.GetComponent<TargetJoint2D>();
                        anchorSet = true;
                        break;
                    }
                }
            }

            //Constantly move the box to cursor
            if (anchorSet && targetJoint != null)
            {
                targetJoint.target = ray;
            }
        }
        else
        {
            if (anchorSet == true)
                anchorSet = false;
        }

    }

}
