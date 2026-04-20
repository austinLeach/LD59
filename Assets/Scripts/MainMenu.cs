using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource menuMusic;

    private void Start()
    {
        if (menuMusic != null)
            menuMusic.Play();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
