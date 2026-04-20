using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class HammerCompletionZone : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;
    [SerializeField] public UnityEvent onAllRepaired;

    private HammerMusicNotes[] requiredNotes;
    private HashSet<HammerMusicNotes> notesInZone = new HashSet<HammerMusicNotes>();
    private BoxCollider2D zone;
    public bool completed = false;
    private bool ready = false;

    private void Awake()
    {
        zone = GetComponent<BoxCollider2D>();
        zone.isTrigger = true;
    }

    private void Start()
    {
        // Call StartMinigame() externally when the minigame begins
    }

    public void StartMinigame()
    {
        completed = false;
        ready = false;
        notesInZone.Clear();
        StartCoroutine(FindNotesAfterDelay());
    }

    private IEnumerator FindNotesAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        requiredNotes = FindObjectsByType<HammerMusicNotes>(FindObjectsSortMode.None);
        Debug.Log($"[HammerCompletionZone] Found {requiredNotes.Length} notes after delay.");
        ready = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HammerMusicNotes note = other.GetComponent<HammerMusicNotes>();
        if (note != null) notesInZone.Add(note);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        HammerMusicNotes note = other.GetComponent<HammerMusicNotes>();
        if (note != null) notesInZone.Remove(note);
    }

    private void Update()
    {
        if (!ready || completed) return;
        if (requiredNotes == null || requiredNotes.Length == 0) return;

        // All notes must be inside the trigger and fully repaired
        if (notesInZone.Count < requiredNotes.Length) return;

        foreach (HammerMusicNotes note in requiredNotes)
        {
            if (note == null || !notesInZone.Contains(note)) return;
            if (note.damageValue != 0) return;
        }

        completed = true;
        foreach (HammerMusicNotes note in requiredNotes)
            if (note != null) Destroy(note.gameObject);
        if (objectToDestroy != null) Destroy(objectToDestroy);
        FindFirstObjectByType<HammerScriptHandler>()?.EnableInterfering();
        onAllRepaired.Invoke();
    }

    public void Reset()
    {
        completed = false;
        ready = false;
        notesInZone.Clear();
        StartCoroutine(FindNotesAfterDelay());
    }
}
