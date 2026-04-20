using System;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1Controller : MonoBehaviour
{
    public PickupBox boxInRange = null;
    public bool hasBoxDeposited = false;

    [SerializeField] private float pianoProgressDuration = 15f;
    [SerializeField] private float guitarProgressDuration = 15f;
    [SerializeField] private float vocalsProgressDuration = 15f;
    [SerializeField] private float drumsProgressDuration = 15f;
    [SerializeField] private float progressDuration = 15f;

    public MiniGameType minigameType = MiniGameType.Piano;

    [Header("Minigame Prefabs")]
    [SerializeField] private GameObject pianoMinigamePrefab;
    [SerializeField] private GameObject vocalsMinigamePrefab;
    [SerializeField] private GameObject guitarMinigamePrefab;
    [SerializeField] private GameObject drumsMinigamePrefab;

    [Header("Spawn Parent (optional)")]
    [SerializeField] private Transform uiParent;

    private GameObject activeMinigame = null;
    private PickupBox depositedBox = null;
    private ProgressBar activeProgressBar;

    // Checkpoints per minigame type
    private static readonly Dictionary<MiniGameType, float[]> checkpoints = new Dictionary<MiniGameType, float[]>
    {
        { MiniGameType.Piano,  new float[] { 0.33f, 0.66f } },
        { MiniGameType.Guitar, new float[] { 0.50f } },
        { MiniGameType.Vocals, new float[] { 0.25f, 0.50f, 0.75f } },
        { MiniGameType.Drums,  new float[] { 0.50f } },
    };

    private HashSet<float> triggeredCheckpoints = new HashSet<float>();
    public bool waitingForMinigame = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        PickupBox box = other.GetComponent<PickupBox>();
        if (box != null)
            boxInRange = box;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PickupBox box = other.GetComponent<PickupBox>();
        if (box != null && box != depositedBox)
            boxInRange = null;
    }

    private void Update()
    {
        switch (minigameType)
        {
            case MiniGameType.Piano: progressDuration = pianoProgressDuration; break;
            case MiniGameType.Guitar: progressDuration = guitarProgressDuration; break;
            case MiniGameType.Vocals: progressDuration = vocalsProgressDuration; break;
            case MiniGameType.Drums: progressDuration = drumsProgressDuration; break;
        }

        if (depositedBox == null || !depositedBox.isProgressing || waitingForMinigame) return;

        depositedBox.progress += Time.deltaTime / progressDuration;
        depositedBox.progress = Mathf.Clamp01(depositedBox.progress);
        activeProgressBar?.SetProgress(depositedBox.progress);

        // Check if we've hit a checkpoint
        if (checkpoints.TryGetValue(minigameType, out float[] points))
        {
            foreach (float point in points)
            {
                if (!triggeredCheckpoints.Contains(point) && depositedBox.progress >= point)
                {
                    triggeredCheckpoints.Add(point);
                    PauseForMinigame();
                    return;
                }
            }
        }

        if (depositedBox.progress >= 1f)
            StopProgress();
    }

    private void PauseForMinigame()
    {
        waitingForMinigame = true;
        depositedBox.isProgressing = false;
        // Player now needs to walk over and press E — 
        // AcceptMinigameInteraction() is called from your existing E press logic
    }

    // Call this from your player interaction (E key) when near the station
    // while waitingForMinigame is true
    public void AcceptMinigameInteraction()
    {
        if (!waitingForMinigame) return;
        LaunchMinigame();
    }

    public bool AcceptBox(PickupBox box)
    {
        if (minigameType != box.minigameType)
        {
            Debug.Log("Minigame has rejected box");
            return false;
        }
        Debug.Log("Minigame has accepted box");
        depositedBox = box;
        hasBoxDeposited = true;
        boxInRange = null;
        triggeredCheckpoints.Clear();
        waitingForMinigame = false;
        box.SnapToStation(transform);
        StartProgress(depositedBox);
        return true;
    }

    public PickupBox TakeBox()
    {
        StopProgress();
        PickupBox box = depositedBox;
        depositedBox = null;
        hasBoxDeposited = false;
        waitingForMinigame = false;
        return box;
    }

    public void StartProgress(PickupBox box)
    {
        depositedBox = box;
        activeProgressBar = box.GetComponent<ProgressBar>();
        if (activeProgressBar != null)
        {
            activeProgressBar.Show();
            activeProgressBar.SetProgress(box.progress);
        }
        box.isProgressing = true;
    }

    public void StopProgress()
    {
        if (depositedBox != null)
            depositedBox.isProgressing = false;
        activeProgressBar = null;
        depositedBox = null;
    }

    private void LaunchMinigame()
    {
        if (minigameType == MiniGameType.Piano)
            SpawnMinigame(pianoMinigamePrefab);
        else if (minigameType == MiniGameType.Vocals)
            SpawnMinigame(vocalsMinigamePrefab);
        else if (minigameType == MiniGameType.Guitar)
            SpawnMinigame(guitarMinigamePrefab);
        else if (minigameType == MiniGameType.Drums)
            SpawnMinigame(drumsMinigamePrefab);
    }

    private void SpawnMinigame(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("Minigame prefab not assigned on " + gameObject.name);
            waitingForMinigame = false;
            return;
        }

        activeMinigame = Instantiate(prefab, uiParent);

        if (minigameType == MiniGameType.Piano)
        {
            PianoMinigameUI ui = activeMinigame.GetComponent<PianoMinigameUI>();
            if (ui != null) ui.OnMinigameFinished += HandleMinigameFinished;
        }
        else if (minigameType == MiniGameType.Vocals)
        {
            MicrophoneDropScript mic = activeMinigame.GetComponentInChildren<MicrophoneDropScript>();
            if (mic != null) mic.OnMicLanded += HandleMinigameFinished;
        }
        else if (minigameType == MiniGameType.Guitar)
        {
            GuitarMinigameSequencer ui = activeMinigame.GetComponent<GuitarMinigameSequencer>();
            if (ui != null) ui.OnMinigameFinished += HandleMinigameFinished;
        }
        else if (minigameType == MiniGameType.Drums)
        {
            // wire up drums here when ready
        }
    }

    private void HandleMinigameFinished(bool won)
    {
        Debug.Log(won ? "Minigame won!" : "Minigame lost.");

        if (activeMinigame != null)
        {
            Destroy(activeMinigame);
            activeMinigame = null;
        }

        // Resume progress regardless of win/lose — 
        // adjust this if losing should reset progress
        waitingForMinigame = false;
        if (depositedBox != null)
        {
            depositedBox.isProgressing = true;
        }
    }
}