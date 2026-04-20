using UnityEngine;
using TMPro;

public class GuitarMinigameSequencer : MonoBehaviour
{
    [Header("Step GameObjects (enabled/disabled per step)")]
    [SerializeField] private GameObject scissorsStep;
    [SerializeField] private GameObject stringBagStep;
    [SerializeField] private GameObject spinStep;

    [Header("Instructions")]
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private string scissorsInstruction = "Cut the old string with the scissors!";
    [SerializeField] private string stringInstruction   = "Drag a new string onto the guitar!";
    [SerializeField] private string spinInstruction     = "Turn the tuning peg 3 times!";
    [SerializeField] private string completeInstruction = "All done!";

    private void Start()
    {
        GoToStep(1);
    }

    // Called by DraggableUI.onSuccess
    public void OnScissorsComplete()
    {
        GoToStep(2);
    }

    // Called by StringBagItem.onSuccess
    public void OnStringComplete()
    {
        GoToStep(3);
    }

    // Called by SpinUI.onSuccess
    public void OnSpinComplete()
    {
        SetInstruction(completeInstruction);
    }

    public void ResetMinigame()
    {
        if (scissorsStep  != null) scissorsStep.SetActive(false);
        if (stringBagStep != null) stringBagStep.SetActive(false);
        if (spinStep      != null) spinStep.SetActive(false);

        // Brief delay so any in-flight destroy calls finish before re-enabling
        GoToStep(1);
    }

    private void GoToStep(int step)
    {
        if (scissorsStep  != null) scissorsStep.SetActive(step == 1);
        if (stringBagStep != null) stringBagStep.SetActive(step == 2);
        if (spinStep      != null) spinStep.SetActive(step == 3);

        switch (step)
        {
            case 1: SetInstruction(scissorsInstruction); break;
            case 2: SetInstruction(stringInstruction);   break;
            case 3: SetInstruction(spinInstruction);     break;
        }
    }

    private void SetInstruction(string text)
    {
        if (instructionText != null)
            instructionText.text = text;
    }
}
