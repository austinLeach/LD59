using System;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1Controller : MonoBehaviour
{
    public enum MiniGameType { Piano, Guitar, Drums }            //add minigame types here
    public PickupBox boxInRange = null;
    public bool hasBoxDeposited = false;

    public MiniGameType minigameType =  MiniGameType.Piano;

    [Header("Minigame Prefabs")]
    [SerializeField] private GameObject pianoMinigamePrefab;   // drag your popup prefab here

    [Header("Spawn Parent (optional)")]
    [SerializeField] private Transform uiParent;               // drag your Canvas here, or leave null

    private GameObject activeMinigame = null;
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
        LaunchMinigame();
    }


    public PickupBox TakeBox()
    {
        PickupBox box = depositedBox;
        depositedBox = null;
        hasBoxDeposited = false;
        return box;
    }


    private void LaunchMinigame()
    {
        if (minigameType == MiniGameType.Piano)
        {
            SpawnMinigame(pianoMinigamePrefab);
        }
            
        // add more types here:
        // else if (minigameType == MiniGameType.Guitar)
        //     SpawnMinigame(guitarMinigamePrefab);
    }

    private void SpawnMinigame(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Minigame prefab not assigned on " + gameObject.name);
            return;
        }

        // Spawn under the Canvas if provided, otherwise at scene root
        activeMinigame = Instantiate(prefab, uiParent);

        // Get the UI controller and subscribe to its completion event
        PianoMinigameUI ui = activeMinigame.GetComponent<PianoMinigameUI>();
        if (ui != null)
            ui.OnMinigameFinished += HandleMinigameFinished;
    }

    private void HandleMinigameFinished(bool won)
    {
        Debug.Log(won ? "Minigame won!" : "Minigame lost.");

        // mess with the box to add completion to it or whatever

        if (activeMinigame != null)
        {
            Destroy(activeMinigame);
            activeMinigame = null;
        }
    }

}
