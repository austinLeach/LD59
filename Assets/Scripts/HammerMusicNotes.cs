using System;
using UnityEngine;

public class HammerMusicNotes : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private TargetJoint2D target2D;
    private bool dragging = false;

    public int damageValue;
    public bool done = false;
    public Sprite[] boxArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        damageValue = UnityEngine.Random.Range(0, 3);
        changeNoteColor();
    }
        
    // Update is called once per frame
    void Update()
    {
        dragging = Input.GetMouseButton(0);
        //TODO: When hammered reduce damage by 1
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

    void repairDamage()
    {
        if(damageValue != 0)
        {
            damageValue = damageValue--;
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
