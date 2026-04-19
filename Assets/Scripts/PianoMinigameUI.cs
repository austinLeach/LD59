using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// uGUI controller for the Piano Tiles minigame popup.
///
/// Scene setup
/// -----------
/// Create a Canvas ? Panel (popup root) and assign the fields below.
///
/// Popup Panel
///   ??? TitleBar
///   ?     ??? TitleText (TMP)
///   ?     ??? ProgressText (TMP)          e.g. "Note 3 of 10"
///   ?     ??? ResetButton (Button)
///   ??? BoardArea
///   ?     ??? ColumnGrid (4 child Panels, one per column)
///   ?           ??? Each column: vertical LayoutGroup, children are tile Images
///   ??? TapButtons (HorizontalLayoutGroup)
///   ?     ??? 4 × Button with child TMP label
///   ??? ResultBar (Panel + TMP, starts inactive)
/// </summary>
public class PianoMinigameUI : MonoBehaviour
{
    [Header("Logic")]
    [SerializeField] private PianoMinigame game;

    [Header("Popup")]
    [SerializeField] private GameObject popupRoot;

    [Header("Board")]
    [SerializeField] private Transform[] columnParents;   // 4 column containers
    [SerializeField] private GameObject tilePrefab;       // Image prefab — assign in Inspector

    [Header("Tap Buttons")]
    [SerializeField] private Button[] tapButtons;         // 4 buttons (A B C D)

    [Header("UI Labels")]
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Button resetButton;

    [Header("Result")]
    [SerializeField] private GameObject resultBar;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private float closeDelay = 2f;

    [Header("Colors")]
    [SerializeField] private Color colorActive = new Color(0.09f, 0.37f, 0.65f); // blue
    [SerializeField] private Color colorUpcoming = new Color(0.85f, 0.85f, 0.85f);
    [SerializeField] private Color colorHit = new Color(0.06f, 0.43f, 0.33f); // green
    [SerializeField] private Color colorEmpty = new Color(0f, 0f, 0f, 0f);
    [SerializeField] private Color colorFlashHit = new Color(0.11f, 0.62f, 0.46f);
    [SerializeField] private Color colorFlashMiss = new Color(0.89f, 0.29f, 0.29f);

    [Header("Board Display")]
    [SerializeField] private int visibleBehind = 1;
    [SerializeField] private int visibleAhead = 3;
    [SerializeField] private int tilesPerColumn = 5; // total tile slots rendered per column

    public event Action<bool> OnMinigameFinished;



    // Cached tile Image references [column][slot]
    private Image[][] _tiles;

    private void Awake()
    {
        game.OnGameStarted += RefreshBoard;
        game.OnNoteAdvanced += _ => RefreshBoard();
        game.OnWrongTap += col => StartCoroutine(FlashButton(col, false));
        game.OnGameWon += () => StartCoroutine(FinishAndClose(true));
        game.OnGameLost += () => StartCoroutine(FinishAndClose(false));

        for (int i = 0; i < tapButtons.Length; i++)
        {
            int col = i; 
            tapButtons[i].onClick.AddListener(() => OnTap(col));
        }

        resetButton.onClick.AddListener(StartGame);

        BuildTilePool();
    }

    private void Start()
    {
        OpenPopup();
    }

    

    public void OpenPopup()
    {
        popupRoot.SetActive(true);
        StartGame();
    }

    public void ClosePopup()
    {
        popupRoot.SetActive(false);
    }

    // ---------------------------------------------------------------
    // Internal
    // ---------------------------------------------------------------

    private void StartGame()
    {
        resultBar.SetActive(true);
        game.StartGame();
    }

    private void OnTap(int col)
    {
        if (game.IsGameOver) return;

        bool wasCorrect = game.GetNoteColumn(game.CurrentNote) == col;
        game.Tap(col);

        StartCoroutine(FlashButton(col, wasCorrect));
    }

    // ---- Board rendering ----

    private void BuildTilePool()
    {
        _tiles = new Image[columnParents.Length][];
        for (int c = 0; c < columnParents.Length; c++)
        {
            // Clear any designer-placed children
            foreach (Transform child in columnParents[c])
                Destroy(child.gameObject);

            _tiles[c] = new Image[tilesPerColumn];
            for (int s = 0; s < tilesPerColumn; s++)
            {
                var go = Instantiate(tilePrefab, columnParents[c]);
                _tiles[c][s] = go.GetComponent<Image>();
            }
        }
    }

    private void RefreshBoard()
    {
        UpdateProgress();

        // Clear all tiles
        for (int c = 0; c < _tiles.Length; c++)
            for (int s = 0; s < _tiles[c].Length; s++)
                _tiles[c][s].color = colorEmpty;

        // Only look ahead — no hit tiles shown, active is at the bottom
        // slot 0 = top (furthest upcoming), slot tilesPerColumn-1 = bottom (active)
        int remaining = game.SequenceLength - game.CurrentNote;
        int notesToShow = Mathf.Min(tilesPerColumn, remaining);

        for (int i = 0; i < notesToShow; i++)
        {
            int noteIndex = game.CurrentNote + i;
            int col = game.GetNoteColumn(noteIndex);

            // i=0 is active (bottom slot), i=1 is next above it, etc.
            int slot = (tilesPerColumn - 1) - i;

            Color tileColor = i == 0 ? colorActive : colorUpcoming;
            _tiles[col][slot].color = tileColor;
        }
    }

    private void UpdateProgress()
    {
        if (progressText != null)
            progressText.text = $"Note {Mathf.Min(game.CurrentNote + 1, game.SequenceLength)} of {game.SequenceLength}";
    }


    private void ShowResult(bool win)
    {
        resultBar.SetActive(true);
        resultText.text = win
            ? "Sequence complete! Lock opened."
            : $"Wrong tile! Reached note {game.CurrentNote} of {game.SequenceLength}.";
        resultText.color = win
            ? new Color(0.06f, 0.43f, 0.33f)
            : new Color(0.64f, 0.18f, 0.18f);
    }

    // ---- Button flash ----

    private IEnumerator FlashButton(int col, bool success)
    {
        if (col < 0 || col >= tapButtons.Length) yield break;
        var img = tapButtons[col].GetComponent<Image>();
        if (img == null) yield break;

        Color original = img.color;
        img.color = success ? colorFlashHit : colorFlashMiss;
        yield return new WaitForSeconds(0.15f);
        img.color = original;
    }

    private IEnumerator FinishAndClose(bool won)
    {
        ShowResult(won);
        yield return new WaitForSeconds(closeDelay);
        OnMinigameFinished?.Invoke(won);
        Destroy(gameObject);
    }
}
