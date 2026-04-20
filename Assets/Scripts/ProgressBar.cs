using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject progressBarRoot;  // the UI panel to show/hide
    [SerializeField] private Image fillImage;             // the fill Image set to Fill type

    private void Awake()
    {
        Show();
    }

    public void Show() => progressBarRoot.SetActive(true);
    public void Hide() => progressBarRoot.SetActive(false);

    public void SetProgress(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value);
    }
}
