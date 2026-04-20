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
        Debug.Log("checking for floor");
        if (collision.CompareTag("floor"))
        {
            Debug.Log("found floor");
            OnMicLanded?.Invoke(false);
        }
        else if (collision.CompareTag("player"))
        {
            OnMicLanded?.Invoke(true);
        }
    }
}
