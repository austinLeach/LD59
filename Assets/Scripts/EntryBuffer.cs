using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;


public class EntryBuffer : MonoBehaviour
{
    public GameObject[] boxTypes;
    public GameObject[] pathObj;
    public GameObject accepter;

    private bool[] entrySlots;
    private int nextBoxType = 0;
    private int currentBox = 0;
    private int currentSetIndex = 0;
    private SubmissionBox subScript;
    private int indexA = 0;
    private int indexB = 0;
    private int boxNum = 0;
    private Vector3 spawnLocation;
    private string whatSlot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        subScript = accepter.GetComponent<SubmissionBox>();
        //init empty spawn slots locations.
        int c = 0;
        while (c < 8)
        {
            entrySlots[c] = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Go through all sets
        while (indexA < subScript.submissionSets.Count)
        {
            SubmissionSet set = subScript.submissionSets[indexA];
            foreach (var req in set.requirements)
            {
                switch (req.boxType)
                {
                    case MiniGameType.Piano:
                        boxNum = 2;
                        break;
                    case MiniGameType.Guitar:
                        boxNum = 1;
                        break;
                    case MiniGameType.Drums:
                        boxNum = 0;
                        break;
                    case MiniGameType.Vocals:
                        boxNum = 3;
                        break;
                }
                //Find open slot to spawn into
                int c = 0; 
                while (c < 8)
                {
                    if (entrySlots[c] == true)
                    {
                        c++;
                        whatSlot = "Slot" + c.ToString();
                    }
                    else {
                        //make slot + number
                    }
                }


                indexB = 0;
                while(indexB < req.amount)
                {
                    //BASED ON C USE THE SLOT LOCATION
                    Instantiate(boxTypes[boxNum],spawnLocation);
                    indexB++;
                }
                //Spawn required boxes until count reached
                
            }
            indexA++;
        }

            
        //Read list of boxes to spawn
        if (nextBoxType == currentBox) {
            //Spawn box

        }
    }
}
