using System;
using System.Collections.Generic;
using UnityEngine;

public class PianoMinigame : MonoBehaviour 
{
    [SerializeField] private int sequenceLength = 10;
    [SerializeField] private int columnCount = 4;

    public int CurrentNote { get; private set; }
    public int SequenceLength => sequenceLength;
    public int ColumnCount => columnCount;
    public bool IsGameOver { get; private set; }
    public bool IsWon { get; private set; }

    public event Action OnGameStarted;
    public event Action<int> OnNoteAdvanced;       // arg: new CurrentNote index
    public event Action<int> OnWrongTap;           // arg: column tapped
    public event Action OnGameWon;
    public event Action OnGameLost;

    private int[] _sequence;
    public void StartGame()
    {
        _sequence = GenerateSequence();
        CurrentNote = 0;
        IsGameOver = false;
        IsWon = false;
        OnGameStarted?.Invoke();
    }

    public void Tap(int column)
    {
        if (IsGameOver) return;
        if (column < 0 || column >= columnCount) return;

        if (column == _sequence[CurrentNote])
        {
            CurrentNote++;
            if (CurrentNote >= sequenceLength)
            {
                IsGameOver = true;
                IsWon = true;
                OnNoteAdvanced?.Invoke(CurrentNote);
                OnGameWon?.Invoke();
            }
            else
            {
                OnNoteAdvanced?.Invoke(CurrentNote);
            }
        }
        else
        {
            IsGameOver = true;
            IsWon = false;
            OnWrongTap?.Invoke(column);
            OnGameLost?.Invoke();
        }
    }

    public int GetNoteColumn(int noteIndex)
    {
        if (_sequence == null || noteIndex < 0 || noteIndex >= _sequence.Length)
            return -1;
        return _sequence[noteIndex];
    }

    public List<(int noteIndex, int column, NoteState state)> GetVisibleNotes(
        int visibleBehind = 1, int visibleAhead = 3)
    {
        var result = new List<(int, int, NoteState)>();
        int start = Mathf.Max(0, CurrentNote - visibleBehind);
        int end = Mathf.Min(sequenceLength - 1, CurrentNote + visibleAhead);

        for (int i = start; i <= end; i++)
        {
            NoteState state;
            if (i < CurrentNote)
            {
                state = NoteState.Hit;
            }
            else if (i == CurrentNote)
            {
                state = NoteState.Active;
            }
            else
            {
                state = NoteState.Upcoming;
            }

            result.Add((i, _sequence[i], state));
        }
        return result;
    }

    private int[] GenerateSequence()
    {
        var seq = new int[sequenceLength];
        for (int i = 0; i < sequenceLength; i++)
        {
            seq[i] = UnityEngine.Random.Range(0, columnCount);
        }
            
        return seq;
    }

}
