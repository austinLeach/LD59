using UnityEngine;

public class HammerScriptHandler : MonoBehaviour
{
    public MusicBox musicBox;
    public GameObject noteBoxHammer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Load the inserted box and spawn notes
        if (musicBox.boxType == 1)
        {
            for (int i = 0; i < musicBox.noteCount; i++)
            {
                Instantiate(noteBoxHammer, new Vector3(i * 2.0f, 0, 0),Quaternion.identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
