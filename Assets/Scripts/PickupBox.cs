using UnityEngine;

public class PickupBox : MonoBehaviour
{
    public bool isBeingCarried = false;
    public Vector3 boxPlayerOffset = new Vector3(12, 0.5f, 0);
    public Vector3 boxStationOffset = new Vector3(8, 0.5f, 0);
    public Vector3 putDownOffset = new Vector3(15, 0.75f, 0);

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetPickedUp(Transform playerTransform)
    {
        isBeingCarried = true;
        rb.bodyType = RigidbodyType2D.Dynamic;    //disable physics left in rigidbody just in case we wanted collision sometime
        rb.linearVelocity = Vector3.zero;
        rb.gravityScale = 0f;
        rb.simulated = false;
        transform.SetParent(playerTransform);       //box is now child of parent
        transform.localPosition = boxPlayerOffset;
    }

    public void getPutDown()
    {
        isBeingCarried = false;
        rb.bodyType = RigidbodyType2D.Dynamic;    //change to .Dynamic if physics would like to be enabled    
        rb.linearVelocity = Vector3.zero;
        rb.gravityScale = 0f;
        rb.simulated = true;
        rb.WakeUp();
        transform.localPosition = putDownOffset;
        transform.SetParent(null);

    }

    public void SnapToStation(Transform stationTransform)
    {
        getPutDown();
        transform.SetParent(stationTransform);
        transform.localPosition = boxStationOffset;
    }
}
