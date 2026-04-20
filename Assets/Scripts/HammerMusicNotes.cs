using System;
using UnityEngine;

public class HammerMusicNotes : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D boxCollider;
    private BoxCollider2D secondBox;
    private TargetJoint2D target2D;
    private bool dragging = false;
    private float timer = 15f;
    private bool waiting = false;

    public GameObject forge;
    public int damageValue;
    public bool done = false;
    public Sprite[] boxArray;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        secondBox = 
        boxCollider = GetComponent<BoxCollider2D>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        damageValue = UnityEngine.Random.Range(0, 3);
        changeNoteColor();
    }
        
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Forge On/Off: " + forge.gameObject.GetComponent<ForgeController>().onOff);
        if (forge.GetComponent<ForgeController>().onOff == true && waiting == false)
        {
        }
        dragging = Input.GetMouseButton(0);
            //onMouse click move and drag
            if (dragging && GetComponent<TargetJoint2D>().anchor != new Vector2(0,0))
            {
                GetComponent<TargetJoint2D>().enabled = true;
            } else
            {
                GetComponent<TargetJoint2D>().anchor = new Vector2(0,0);
                GetComponent<TargetJoint2D>().enabled = false;
            }     
    }
    public void repairDamage()
    {
        Debug.Log("I am REPAIRING!");
        if(damageValue != 0)
        {
            damageValue--;
            changeNoteColor();
        }
        else
        {
            damageValue = 2;
            changeNoteColor();
        }
    }

    void changeNoteColor()
    {
        switch (damageValue)
        {
            case 0:
                Debug.Log("Changing to green");
                SpriteRenderer.sprite = boxArray[0];
                SpriteRenderer.color = Color.green;
                break;
            case 1:
                Debug.Log("Changing to yellow");
                SpriteRenderer.sprite = boxArray[1];
                SpriteRenderer.color = Color.yellow;
                break;
            case 2:
                Debug.Log("Changing to orange");
                SpriteRenderer.sprite = boxArray[1];
                SpriteRenderer.color = new Color(1f, 0.5f, 0f);
                break;
            case 3:
                Debug.Log("Changing to red");
                SpriteRenderer.sprite = boxArray[2];
                SpriteRenderer.color = Color.red;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("forge"))
        {
            Debug.Log("I am being hit!");
            repairDamage();
        }
    }
}
