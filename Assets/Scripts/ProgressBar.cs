using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private GameObject progressBarRoot;  // the UI panel to show/hide
    [SerializeField] private Image fillImage;             // the fill Image set to Fill type
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float rotationAngle = 20f;
    private float _rotationDirection = 1f;
    private float _currentAngle = 0f;
    [SerializeField] private Vector2 rotationPivot = new Vector2(0.5f, 0.5f);

    private void Awake()
    {
        Hide();
        if (exclamationMark != null)
        {
            exclamationMark.SetActive(false);
            RectTransform rt = exclamationMark.GetComponent<RectTransform>();
            if (rt != null)
                rt.pivot = rotationPivot;
        }
        if (checkMark != null)
            checkMark.SetActive(false);

    }

    public void ShowComplete()
    {
        Debug.Log("ShowComplete called");
        progressBarRoot.SetActive(false);
        if (exclamationMark != null) exclamationMark.SetActive(false);
        if (checkMark != null)
        {
            Debug.Log("Setting checkmark active");
            checkMark.SetActive(true);
        }
        else
        {
            Debug.Log("checkMark is NULL");
        }
    }


    private void Update()
    {
        if (exclamationMark == null || !exclamationMark.activeSelf) return;

        _currentAngle += rotationSpeed * _rotationDirection * Time.deltaTime;

        if (_currentAngle >= rotationAngle)
        {
            _currentAngle = rotationAngle;
            _rotationDirection = -1f;
        }
        else if (_currentAngle <= -rotationAngle)
        {
            _currentAngle = -rotationAngle;
            _rotationDirection = 1f;
        }

        exclamationMark.transform.localRotation = Quaternion.Euler(0f, 0f, _currentAngle);
    }

    public void Show() => progressBarRoot.SetActive(true);
    public void Hide() => progressBarRoot.SetActive(false);

    public void SetProgress(float value)
    {
        fillImage.fillAmount = Mathf.Clamp01(value);
    }

    public void SetPaused(bool paused)
    {
        if (exclamationMark != null)
            exclamationMark.SetActive(paused);
    }
}
