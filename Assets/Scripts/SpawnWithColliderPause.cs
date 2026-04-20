using System.Collections;
using UnityEngine;

public class SpawnWithColliderPause : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Vector3 spawnPosition;

    private Collider2D[] allColliders;
    private bool collidersDisabled = false;
    private bool listeningToZone = false;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        if (!collidersDisabled) return;
        HammerCompletionZone zone = FindFirstObjectByType<HammerCompletionZone>();
        if (zone != null && !listeningToZone)
        {
            zone.onAllRepaired.AddListener(EnableColliders);
            listeningToZone = true;
        }
    }

    private void EnableColliders()
    {
        collidersDisabled = false;
        foreach (Collider2D col in allColliders)
            if (col != null) col.enabled = true;
        Destroy(gameObject);
    }

    private IEnumerator SpawnRoutine()
    {
        // Disable all colliders in the scene
        allColliders = FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        foreach (Collider2D col in allColliders)
            col.enabled = false;
        collidersDisabled = true;

        // Wait one physics frame so nothing can push the spawned object
        yield return new WaitForFixedUpdate();

        // Spawn the prefab
        Instantiate(prefabToSpawn, new Vector3(1.41f, 0.32f, 0f), Quaternion.identity);


    }
}
