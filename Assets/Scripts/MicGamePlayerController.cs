using UnityEngine;
using System;

public class MicGamePlayerController : MonoBehaviour
{
    //movement
    Vector2 direction;
    float horizontal;
    float vertical;

    //public float pickupRange = 1.5f;
    //public KeyCode pickupKey = KeyCode.E;
    //float lastPickupTime = 0f;
    //public float pickupCooldown = 0.2f;

    //private Minigame1Controller nearbyMinigame = null;
    //private PickupBox carriedBox = null;
    private Rigidbody2D rb;
    //private BoxCollider2D bc;
    private CircleCollider2D bc;

    private Animator _animator;
    //private SpriteRenderer _sr;
    private RectTransform _rt;
    private Vector3 _originalScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        //bc = GetComponent<BoxCollider2D>();
        bc = GetComponent<CircleCollider2D>();
        bc.isTrigger = true;
        _animator = GetComponent<Animator>();
        //_sr = GetComponent<SpriteRenderer>();
        _rt = GetComponent<RectTransform>();
        _originalScale = _rt.localScale;
    }

    private void Update()
    {
        //HandlePickup();
        horizontal = Input.GetAxis("Horizontal");
        //vertical = Input.GetAxis("Vertical");
        bool isMoving = horizontal != 0 || vertical != 0;
        _animator.SetBool("isMoving", isMoving);

        // Flip sprite based on horizontal direction
        if (horizontal != 0)
        {
            //_sr.flipX = horizontal < 0;
            Vector3 scale = _originalScale;
            scale.x = horizontal < 0 ? -_originalScale.x : _originalScale.x;
            _rt.localScale = scale;
            /*if(horizontal < 0)
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
                carriedBox.boxPlayerOffset.x = (Math.Abs(carriedBox.boxPlayerOffset.x));
                carriedBox.putDownOffset.x = (Math.Abs(carriedBox.putDownOffset.x));
                carriedBox.UpdateDirection();
            }*/
        }
            

        
    }

    private void FixedUpdate()
    {
        Vector2 position = rb.position;
        position.x = position.x + 350f * horizontal * Time.deltaTime;
        position.y = position.y + 10f * vertical * Time.deltaTime;
        rb.MovePosition(position);
        bool isMoving = horizontal != 0 || vertical != 0;
        _animator.SetBool("isMoving", isMoving);
    }

    private void HandlePickup()
    {
        //if (!Input.GetKeyDown(pickupKey)) return;

        //if (Time.time - lastPickupTime < pickupCooldown) return;
        //lastPickupTime = Time.time;

        //PickupBox nearbyBox = FindNearbyBox();


        /*if (Input.GetKeyDown(pickupKey))
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
        }*/
    }

    //PickupBox FindNearbyBox()
    //{
        /*Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange);

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
        return null;*/
    //}

    private void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRange);*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*Minigame1Controller minigame = collision.GetComponent<Minigame1Controller>();
        if(minigame != null)
        {
            nearbyMinigame = minigame;
        }*/
        MicrophoneDropScript microphone = collision.GetComponent<MicrophoneDropScript>();
        if(microphone != null)
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*Minigame1Controller minigame = collision.GetComponent<Minigame1Controller>();
        if(minigame != null && minigame == nearbyMinigame)
        {
            nearbyMinigame = null;
        }*/
    }
}
