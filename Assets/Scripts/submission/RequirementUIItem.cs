using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RequirementUIItem : MonoBehaviour
{
    public Image icon;
    public TMP_Text countText;

    private int requiredAmount;

    public void Setup(Sprite sprite, int required)
    {
        icon.sprite = sprite;
        requiredAmount = required;
        UpdateCount(0);
    }

    public void UpdateCount(int current)
    {
        countText.text = current + " / " + requiredAmount;

        if (current >= requiredAmount)
        {
            countText.color = Color.green;
        }
    }
}
