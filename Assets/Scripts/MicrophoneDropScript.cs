using UnityEngine;

public class MicrophoneDropScript : MonoBehaviour
{
    private void Start()
    {
        Vector3 pos = transform.position;
        pos.x = Random.Range(-625f, 625f);
        transform.localPosition = pos;
    }


}
