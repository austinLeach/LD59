using System;
using System.Collections.Generic;
using UnityEngine;

public class Minigame1Controller : MonoBehaviour
{
    public PickupBox boxInRange = null;
    public bool hasBoxDeposited = false;

    private float pianoProgressDuration = 7f;
    private float guitarProgressDuration = 8f;
    private float vocalsProgressDuration = 5f;
    private float drumsProgressDuration = 4f;
    public float progressDuration = 15f;

    PlayerController playerController = null;

    public MiniGameType minigameType = MiniGameType.Piano;

    [Header("Minigame Prefabs")]
    [SerializeField] private GameObject pianoMinigamePrefab;
    [SerializeField] private GameObject vocalsMinigamePrefab;
    [SerializeField] private GameObject guitarMinigamePrefab;
    [SerializeField] private GameObject drumsMinigamePrefab;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip failSound;
    [SerializeField] private AudioClip winSound;

    [Header("Spawn Parent (optional)")]
    [SerializeField] private Transform uiParent;

    private GameObject activeMinigame = null;
    public PickupBox depositedBox = null;
    private ProgressBar activeProgressBar;

    // Checkpoints per minigame type
    private static readonly Dictionary<MiniGameType, float[]> checkpoints = new Dictionary<MiniGameType, float[]>
    {
        { MiniGameType.Piano,  new float[] { 0.33f, 0.66f } },
        { MiniGameType.Guitar, new float[] { 0.50f } },
        //{ MiniGameType.Vocals, new float[] { 0.25f, 0.50f, 0.75f } },
        { MiniGameType.Vocals, new float[] { 0.33f, 0.66f } },
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

    void Start()
    {
        Debug.Log("in start setting number is " + progressDuration + " currently");
        if (minigameType == MiniGameType.Drums)
        {
            progressDuration = drumsProgressDuration;
        }
        else if (minigameType == MiniGameType.Guitar)
        {
            progressDuration = guitarProgressDuration;
        }
        else if (minigameType == MiniGameType.Vocals)
        {
            progressDuration = vocalsProgressDuration;
        }
        else
        {
            Debug.Log("in piano code, pianoprogressduration is " + pianoProgressDuration);
            progressDuration = pianoProgressDuration;
        }
        Debug.Log("in start just set to " + progressDuration + " currently");
    }

    private void Update()
    {

        

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
        activeProgressBar?.SetPaused(true);
    }

    // Call this from your player interaction (E key) when near the station
    // while waitingForMinigame is true
    public void AcceptMinigameInteraction(PlayerController playerControllerPassed)
    {
        if (!waitingForMinigame) return;
        playerController = playerControllerPassed;
        playerController.gameObject.SetActive(false);
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
        // Stop without showing complete

        if(depositedBox != null)
        {
            Debug.Log("DEPOSIT BOX IS NOT NULL");
        }
        else
        {
            Debug.Log("DEPOSIT BOX IS NULL");
        }
        if (depositedBox != null)
            depositedBox.isProgressing = false;
        activeProgressBar = null;

        PickupBox box = depositedBox;
        depositedBox = null;
        hasBoxDeposited = false;
        waitingForMinigame = false;

        if(box != null)
        {
            Debug.Log("BOX BEING HANDED OUT IS NOT NULL");
        }
        else
        {
            Debug.Log("BOX BEING HANDED OUT IS NULL");
        }

        
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
            activeProgressBar.SetPaused(false); // make sure exclamation is hidden on deposit
        }
        box.isProgressing = true;
    }

    public void StopProgress()
    {
        Debug.Log("StopProgress called");
        if (depositedBox != null)
            depositedBox.isProgressing = false;

        ProgressBar bar = activeProgressBar;
        Debug.Log("activeProgressBar is: " + (bar == null ? "NULL" : "valid"));
        activeProgressBar = null;
        //depositedBox = null;

        bar?.ShowComplete();
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
            HammerCompletionZone ui = activeMinigame.GetComponentInChildren<HammerCompletionZone>();
            if (ui != null) ui.OnMinigameFinished += HandleMinigameFinished;
        }
    }

    private void HandleMinigameFinished(bool won)
    {
        
        
        Debug.Log(won ? "Minigame won!" : "Minigame lost.");

        playerController.gameObject.SetActive(true);
        //playerController = null;

        if(won)
        {
            Debug.Log("made it into win");
            audioSource.PlayOneShot(winSound, 0.2f);
        }
        else
        {
            Debug.Log("made it into fail");
            audioSource.PlayOneShot(failSound);
        }

        if (activeMinigame != null)
        {
            Destroy(activeMinigame);
            activeMinigame = null;
        }
        Debug.Log("made it into about to relaunch");
        if (!won)
        {
            LaunchMinigame();
            return;
        }

        waitingForMinigame = false;
        activeProgressBar?.SetPaused(false);  // hide exclamation when resuming
        if (depositedBox != null)
        {
            depositedBox.isProgressing = true;
        }

    }
}