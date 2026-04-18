using UnityEngine;

public class Minigame1Controller : MonoBehaviour
{
    public PickupBox boxInRange = null;
    public bool hasBoxDeposited = false;

    private PickupBox depositedBox = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        PickupBox box = other.GetComponent<PickupBox>();
        if (box != null)
        {
            boxInRange = box;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PickupBox box = other.GetComponent<PickupBox>();
        if (box != null && box != depositedBox)
        {
            boxInRange = null;
        }
    }

    public void AcceptBox(PickupBox box)
    {
        Debug.Log("Minigame has accepted box");
        depositedBox = box;
        hasBoxDeposited = true;
        boxInRange = null;
        box.SnapToStation(transform);
        // minigame game logic here
    }

    public PickupBox TakeBox()
    {
        PickupBox box = depositedBox;
        depositedBox = null;
        hasBoxDeposited = false;
        return box;
    }
}
