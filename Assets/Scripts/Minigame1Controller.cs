using System;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1Controller : MonoBehaviour
{
    //public enum MiniGameType { Piano, Guitar, Drums, Vocals}            //add minigame types here
    public PickupBox boxInRange = null;
    public bool hasBoxDeposited = false;

    [SerializeField] private float progressDuration = 3f; // adjust per minigame type
    private float progressTimer = 0f;
    private bool isProgressing = false;
    private ProgressBar activeProgressBar;

    public MiniGameType minigameType =  MiniGameType.Piano;

    [Header("Minigame Prefabs")]
    [SerializeField] private GameObject pianoMinigamePrefab;   // drag your popup prefab here
    [SerializeField] private GameObject vocalsMinigamePrefab;
    [SerializeField] private GameObject guitarMinigamePrefab;
    [SerializeField] private GameObject drumsMinigamePrefab;

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

    private void Update()
    {
        if (!isProgressing || activeProgressBar == null) return;

        progressTimer += Time.deltaTime;
        float progress = progressTimer / progressDuration;
        activeProgressBar.SetProgress(progress);

        if (progressTimer >= progressDuration)
            StopProgress();
    }

    public bool AcceptBox(PickupBox box)
    {
        if(minigameType != box.minigameType)
        {
            Debug.Log("Minigame has rejected box");
            return false;
        }
        Debug.Log("Minigame has accepted box");
        depositedBox = box;
        hasBoxDeposited = true;
        boxInRange = null;
        box.SnapToStation(transform);
        StartProgress(depositedBox);
        LaunchMinigame();
        return true;
    }


    public PickupBox TakeBox()
    {
        StopProgress();
        PickupBox box = depositedBox;
        depositedBox = null;
        hasBoxDeposited = false;
        return box;
    }

    public void StartProgress(PickupBox box)
    {
        progressTimer = 0f;
        isProgressing = true;
        activeProgressBar = box.GetComponent<ProgressBar>();
        if (activeProgressBar != null)
            activeProgressBar.Show();
    }

    public void StopProgress()
    {
        isProgressing = false;
        if (activeProgressBar != null)
            activeProgressBar.Hide();
        activeProgressBar = null;
    }

    private void LaunchMinigame()
    {
        if (minigameType == MiniGameType.Piano)
        {
            SpawnMinigame(pianoMinigamePrefab);
        }
        else if (minigameType == MiniGameType.Vocals)
        {
            SpawnMinigame(vocalsMinigamePrefab);
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
        if(minigameType == MiniGameType.Piano)
        {
            PianoMinigameUI ui = activeMinigame.GetComponent<PianoMinigameUI>();
            if (ui != null)
                ui.OnMinigameFinished += HandleMinigameFinished;
        }
        else if(minigameType == MiniGameType.Vocals)
        {
            MicrophoneDropScript mic = activeMinigame.GetComponentInChildren<MicrophoneDropScript>();
            if (mic != null)
                mic.OnMicLanded += HandleMinigameFinished;
        }
        
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
