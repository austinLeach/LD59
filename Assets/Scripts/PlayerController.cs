using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float pickupRange = 1.5f;
    public KeyCode pickupKey = KeyCode.E;
    float lastPickupTime = 0f;
    public float pickupCooldown = 0.2f;

    private Minigame1Controller nearbyMinigame = null;
    private PickupBox carriedBox = null;
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }

    private void Update()
    {
        HandlePickup();
    }

    private void HandlePickup()
    {
        if (!Input.GetKeyDown(pickupKey)) return;

        if (Time.time - lastPickupTime < pickupCooldown) return;
        lastPickupTime = Time.time;

        PickupBox nearbyBox = FindNearbyBox();


        if (Input.GetKeyDown(pickupKey))
        {


            if(carriedBox != null && nearbyMinigame != null && !nearbyMinigame.hasBoxDeposited)
            {
                Debug.Log("Trying to deposit box to minigame");
                nearbyMinigame.AcceptBox(carriedBox);
                carriedBox = null;
                return;
            }
            else if(carriedBox != null)         //put down box if holding one
            {
                Debug.Log("Putting down box");
                carriedBox.getPutDown();
                carriedBox = null;
                return;
            }
            else if (nearbyBox != null && nearbyMinigame != null)
            {
                Debug.Log("Picking up box from minigame");
                carriedBox = nearbyMinigame.TakeBox();
                carriedBox.GetPickedUp(transform);
            }
            else if (nearbyBox != null)
            {
                Debug.Log("Picking up box");
                if(nearbyBox != null)
                {
                    carriedBox = nearbyBox;
                    carriedBox.GetPickedUp(transform);
                }
            }
            else
            {
                Debug.Log("No action found");
            }
        }
    }

    PickupBox FindNearbyBox()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange);

        if(hits.Length > 0)
        {
            Debug.Log("at least one box found, hits length: " + hits.Length);
        }

        foreach(Collider2D hit in hits)
        {
            PickupBox box = null;
            try
            {
                box = hit.GetComponent<PickupBox>();
            }
            catch(Exception e)
            {

            }
            
            if(box != null && !box.isBeingCarried)
            {
                return box;
            }
        }
        Debug.Log("returning null");
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Minigame1Controller minigame = collision.GetComponent<Minigame1Controller>();
        if(minigame != null)
        {
            nearbyMinigame = minigame;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Minigame1Controller minigame = collision.GetComponent<Minigame1Controller>();
        if(minigame != null && minigame == nearbyMinigame)
        {
            nearbyMinigame = null;
        }
    }
}
