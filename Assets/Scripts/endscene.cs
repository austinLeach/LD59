using TMPro;
using UnityEngine;

public class totalCoinsAtEnd : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI totalscoretext; // Assign your TMP text component in inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Update the text with the global total collected value when scene loads
        UpdateTotalCollectedDisplay();
    }

    private void UpdateTotalCollectedDisplay()
    {
        if (totalscoretext != null)
        {
            totalscoretext.text = totalscoretext.text + GlobalVariables.score.ToString() + "/15";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}