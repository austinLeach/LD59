using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubmissionBox : MonoBehaviour
{
    public List<SubmissionSet> submissionSets;
    private int currentSetIndex = 0;

    private Dictionary<MiniGameType, int> currentProgress = new Dictionary<MiniGameType, int>();

    public float timer;
    private float overallTimer;
    private float overallTimerMax;
    private bool isActive = false;

    public Image timerBar; // UI Image (fill)
    public Image overallTimerBar;

    public Transform uiContainer;
    public RequirementUIItem uiItemPrefab;
    public List<BoxTypeIcon> boxIcons;

    public AudioSource audioSource;
    [SerializeField] private AudioClip successAudio;
    [SerializeField] private AudioSource failAudio;

    private Dictionary<MiniGameType, RequirementUIItem> uiItems = new Dictionary<MiniGameType, RequirementUIItem>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        overallTimerMax = 0f;
        foreach (var set in submissionSets)
            overallTimerMax += set.timeLimit;

        overallTimer = overallTimerMax;
        StartNextSet();
    }

    void Update()
    {
        if (!isActive) return;

        timer -= Time.deltaTime;
        overallTimer -= Time.deltaTime;
        overallTimer = Mathf.Max(0f, overallTimer);

        UpdateTimerUI();

        if (timer <= 0f)
            FailSet();
    }

    void StartNextSet()
    {
        if (currentSetIndex >= submissionSets.Count)
        {
            Debug.Log("All sets complete!");
            return;
        }

        // Clear old UI
        foreach (Transform child in uiContainer)
        {
            if (child.GetComponent<RequirementUIItem>() != null)
                Destroy(child.gameObject);
        }

        uiItems.Clear();
        currentProgress.Clear();

        SubmissionSet set = submissionSets[currentSetIndex];

        foreach (var req in set.requirements)
        {
            currentProgress[req.boxType] = 0;

            // Create UI element
            RequirementUIItem item = Instantiate(uiItemPrefab, uiContainer);

            Sprite icon = GetIcon(req.boxType);
            Debug.Log($"Getting icon for {req.boxType}: {(icon == null ? "NULL" : "found")}");
            item.Setup(icon, req.amount);

            uiItems[req.boxType] = item;
        }

        timer = set.timeLimit;
        isActive = true;
    }

    Sprite GetIcon(MiniGameType type)
    {
        foreach (var entry in boxIcons)
        {
            if (entry.type == type)
                return entry.icon;
        }

        return null;
    }

    void UpdateTimerUI()
    {
        SubmissionSet set = submissionSets[currentSetIndex];
        timerBar.fillAmount = timer / set.timeLimit;

        if (overallTimerBar != null)
            overallTimerBar.fillAmount = overallTimer / overallTimerMax;
    }

    public bool SubmitBox(PickupBox box)
    {
        if (!isActive) return false;

        SubmissionSet set = submissionSets[currentSetIndex];

        foreach (var req in set.requirements)
        {
            if (req.boxType == box.minigameType)
            {
                if ((currentProgress[req.boxType] < req.amount))
                {
                    currentProgress[box.minigameType]++;
                    uiItems[box.minigameType].UpdateCount(currentProgress[box.minigameType]);
                    CheckCompletion();
                    return true;
                }
                else
                {
                    CheckCompletion();
                    return false;
                }
            }
        }
        return false;
        // Optional: wrong box penalty
        Debug.Log("Wrong box submitted!");
    }

    void CheckCompletion()
    {
        SubmissionSet set = submissionSets[currentSetIndex];

        foreach (var req in set.requirements)
        {
            if (currentProgress[req.boxType] < req.amount)
                return;
        }

        SuccessSet();
    }

    void SuccessSet()
    {
        Debug.Log("SUCCESS!");
        audioSource.PlayOneShot(successAudio, 0.5f);
        isActive = false;

        StartCoroutine(WaitForTimerThenNext());
    }

    IEnumerator WaitForTimerThenNext()
    {
        // Wait for the remaining set timer to drain to zero
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            timer = Mathf.Max(0f, timer);
            UpdateTimerUI();
            yield return null;
        }

        currentSetIndex++;
        StartNextSet();
    }

    void FailSet()
    {
        Debug.Log("FAILED!");

        audioSource.PlayOneShot(successAudio, 0.5f);
        isActive = false;

        StartCoroutine(WaitAndRestartSet());
    }

    IEnumerator WaitAndStartNext()
    {
        yield return new WaitForSeconds(2f);

        currentSetIndex++;
        StartNextSet();
    }

    IEnumerator WaitAndRestartSet()
    {
        yield return new WaitForSeconds(2f);

        StartNextSet();
    }

    public bool TurnInBox(PickupBox box)
    {
        if (box != null)
        {
            bool result = SubmitBox(box);

            if(result)
            {
                Destroy(box.gameObject);
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    

}