using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    //movement
    Vector2 direction;
    float horizontal;
    float vertical;

    public float pickupRange = 1.5f;
    public KeyCode pickupKey = KeyCode.E;
    float lastPickupTime = 0f;
    public float pickupCooldown = 0.2f;

    private Minigame1Controller nearbyMinigame = null;
    private PickupBox carriedBox = null;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Animator _animator;
    private SpriteRenderer _sr;
    private AudioSource audioSource;
    [SerializeField] private AudioClip rejectSound;
    [SerializeField] private LayerMask boundaryLayers;
    private Vector2 baseColliderOffset;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
        baseColliderOffset = bc.offset;
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandlePickup();
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        bool isMoving = horizontal != 0 || vertical != 0;
        _animator.SetBool("isMoving", isMoving);

        // Flip sprite based on horizontal direction
        if (horizontal != 0)
        {
            _sr.flipX = horizontal < 0;
            bc.offset = new Vector2(horizontal < 0 ? -baseColliderOffset.x : baseColliderOffset.x, baseColliderOffset.y);
            if(horizontal < 0)
            {
                if(carriedBox != null)
                {
                    carriedBox.boxPlayerOffset.x = (Math.Abs(carriedBox.boxPlayerOffset.x) * -1);
                    carriedBox.putDownOffset.x = (Math.Abs(carriedBox.putDownOffset.x) * -1);
                    carriedBox.UpdateDirection();
                }
            }
            else if(horizontal > 0)
            {
                if(carriedBox != null)
                {
                    carriedBox.boxPlayerOffset.x = (Math.Abs(carriedBox.boxPlayerOffset.x));
                    carriedBox.putDownOffset.x = (Math.Abs(carriedBox.putDownOffset.x));
                    carriedBox.UpdateDirection();
                }
            }
        }
            

        

    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        Vector2 size = bc.size * (Vector2)transform.lossyScale;
        Vector2 offset = (Vector2)transform.TransformPoint(bc.offset) - rb.position;

        Vector2 nextX = new Vector2(position.x + 10f * horizontal * Time.deltaTime, position.y);
        if (boundaryLayers == 0 || Physics2D.OverlapBox(nextX + offset, size * 0.95f, 0f, boundaryLayers) == null)
            position.x = nextX.x;

        Vector2 nextY = new Vector2(position.x, position.y + 10f * vertical * Time.deltaTime);
        if (boundaryLayers == 0 || Physics2D.OverlapBox(nextY + offset, size * 0.95f, 0f, boundaryLayers) == null)
            position.y = nextY.y;

        rb.MovePosition(position);
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
                bool result = nearbyMinigame.AcceptBox(carriedBox);
                if(result)
                {
                    carriedBox = null;
                }
                else
                {
                    audioSource.PlayOneShot(rejectSound);
                }
                
                return;
            }
            else if(carriedBox != null)         //put down box if holding one
            {
                Debug.Log("Putting down box");
                carriedBox.getPutDown();
                carriedBox = null;
                return;
            }
            else if (nearbyMinigame != null && nearbyMinigame.waitingForMinigame == true)
            {
                nearbyMinigame.AcceptMinigameInteraction();
            }
            else if (carriedBox == null && nearbyMinigame != null && nearbyMinigame.hasBoxDeposited)
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
