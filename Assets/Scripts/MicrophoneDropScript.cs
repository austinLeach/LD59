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
            Debug.Log("am in win script");
            OnMicLanded?.Invoke(true);
        }

        if (collision.gameObject.CompareTag("floor"))
        {
            Debug.Log("am in fail script");
            OnMicLanded?.Invoke(false);
        }


    }
}
