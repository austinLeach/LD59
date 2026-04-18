using System;
using UnityEngine;

public class HammerMusicNotes : MonoBehaviour
{
    SpriteRenderer SpriteRenderer;
    public int damageValue;
    public bool done = false;
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
        //TODO: When hammered reduce damage by 1
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
                SpriteRenderer.color = Color.green;
                break;
            case 1:
                SpriteRenderer.color = Color.yellow;
                break;
            case 2:
                SpriteRenderer.color = Color.orange;
                break;
            case 3:
                SpriteRenderer.color = Color.red;
                break;
        }
    }
}
