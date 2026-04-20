using System;
using Unity.VisualScripting;
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
            Debug.Log("We are so CLOSE");
            if (Physics2D.IsTouching(boxCollider, secondBox))
            {
                Debug.Log("We are touching!");
                repairDamage();
                waiting = true;
                timer = 15f;
                GlobalVariables.Timer(ref waiting,ref timer);
            }
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
            damageValue = damageValue--;
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
                SpriteRenderer.sprite = boxArray[0];
                SpriteRenderer.color = Color.green;
                break;
            case 1:
                SpriteRenderer.sprite = boxArray[1];
                SpriteRenderer.color = Color.yellow;
                break;
            case 2:
                SpriteRenderer.sprite = boxArray[1];
                SpriteRenderer.color = Color.orange;
                break;
            case 3:
                SpriteRenderer.sprite = boxArray[2];
                SpriteRenderer.color = Color.red;
                break;
        }
    }
}
