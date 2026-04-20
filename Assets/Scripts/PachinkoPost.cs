using UnityEngine;

public class PachinkoPost : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float soundCooldown = 0.15f;

    private float _lastSoundTime = -999f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - _lastSoundTime < soundCooldown) return;
        _lastSoundTime = Time.time;
        audioSource.PlayOneShot(hitSound);
    }
}
