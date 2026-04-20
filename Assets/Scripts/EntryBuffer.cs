using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class EntryBuffer : MonoBehaviour
{
    List<GameObject> entryBoxes = new List<GameObject>();
    public GameObject[] entryTargets;
    public GameObject[] boxTypes;
    public float timer = 60f;

    private bool busy = false;
    private int x = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //Spawn box
        //Instantiate(boxTypes[x], entryTargets[0].transform.position, Quaternion.identity);

        //move box quickly from path to path via children
        //once it reaches Path5 it will teleport to the first available slot
    }

    // Update is called once per frame
    void Update()
    {
        if (busy == false)
        {
            busy = true;
            GlobalVariables.Timer(ref busy, ref timer);
            Instantiate(boxTypes[x], entryTargets[0].transform.position, Quaternion.identity);         
        } 

        
    }
}
