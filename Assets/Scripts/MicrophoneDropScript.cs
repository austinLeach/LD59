using UnityEngine;
using System;

public class MicrophoneDropScript : MonoBehaviour
{
    public event Action<bool> OnMicLanded;

    private void Start()
    {
        Vector3 pos = transform.position;
        pos.x = UnityEngine.Random.Range(-625f, 625f);
        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MicGamePlayerController player = collision.GetComponent<MicGamePlayerController>();
        if (player != null)
        {
            OnMicLanded?.Invoke(true);
        }

        if (collision.gameObject.CompareTag("floor"))
        {
            OnMicLanded?.Invoke(false);
        }


    }
}
