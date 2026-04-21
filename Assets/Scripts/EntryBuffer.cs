using UnityEngine;
using System.Collections.Generic;


public class EntryBuffer : MonoBehaviour
{
    public GameObject[] boxTypes;
    public GameObject pathRoot;
    public GameObject slotRoot; // slot1 root — children are slot2, slot3, etc.
    public GameObject accepter;
    public float moveSpeed = 2f;

    private List<Transform> pathNodes = new List<Transform>();
    private List<Transform> slotNodes = new List<Transform>();
    private bool[] slotOccupied;
    private SubmissionBox subScript;
    private Queue<int> spawnQueue = new Queue<int>();
    private bool spawnNodeOccupied = false;

    private class ConveyorBox
    {
        public GameObject obj;
        public int targetNodeIndex;
        public int assignedSlot = -1; // slot index once placed, -1 while on conveyor
    }
    private List<ConveyorBox> activeBoxes = new List<ConveyorBox>();

    void Start()
    {
        subScript = accepter.GetComponent<SubmissionBox>();

        // Walk path hierarchy
        Transform node = pathRoot.transform;
        while (node != null)
        {
            pathNodes.Add(node);
            node = node.childCount > 0 ? node.GetChild(0) : null;
        }

        // Walk slot hierarchy
        Transform slot = slotRoot.transform;
        while (slot != null)
        {
            slotNodes.Add(slot);
            slot = slot.childCount > 0 ? slot.GetChild(0) : null;
        }
        slotOccupied = new bool[slotNodes.Count];

        BuildSpawnQueue();
    }

    // Call this when a new set starts
    public void SpawnBoxesForCurrentSet()
    {
        BuildSpawnQueue();
    }

    void BuildSpawnQueue()
    {
        spawnQueue.Clear();
        if (subScript.submissionSets.Count == 0) return;

        SubmissionSet set = subScript.submissionSets[subScript.currentSetIndex];
        foreach (var req in set.requirements)
        {
            int boxNum;
            switch (req.boxType)
            {
                case MiniGameType.Piano:  boxNum = 2; break;
                case MiniGameType.Guitar: boxNum = 1; break;
                case MiniGameType.Drums:  boxNum = 0; break;
                case MiniGameType.Vocals: boxNum = 3; break;
                default: continue;
            }
            for (int i = 0; i < req.amount; i++)
                spawnQueue.Enqueue(boxNum);
        }
    }

    void Update()
    {
        // Spawn next box at path[0] if the spawn node is free
        if (!spawnNodeOccupied && spawnQueue.Count > 0)
        {
            int boxNum = spawnQueue.Dequeue();
            spawnNodeOccupied = true;
            GameObject box = Instantiate(boxTypes[boxNum], pathNodes[0].position, Quaternion.identity);
            activeBoxes.Add(new ConveyorBox { obj = box, targetNodeIndex = 1 });
        }

        // Move boxes along the path
        for (int i = activeBoxes.Count - 1; i >= 0; i--)
        {
            ConveyorBox cb = activeBoxes[i];
            if (cb.obj == null) { activeBoxes.RemoveAt(i); continue; }

            PickupBox pb = cb.obj.GetComponent<PickupBox>();

            // If a slotted box got picked up, free its slot and stop tracking it
            if (cb.assignedSlot != -1 && pb != null && pb.isBeingCarried)
            {
                slotOccupied[cb.assignedSlot] = false;
                activeBoxes.RemoveAt(i);
                continue;
            }

            if (cb.assignedSlot != -1) continue; // placed and not carried, skip

            // If picked up from the conveyor, stop tracking entirely
            if (pb != null && pb.isBeingCarried)
            {
                activeBoxes.RemoveAt(i);
                continue;
            }

            Vector3 target = pathNodes[cb.targetNodeIndex].position;
            cb.obj.transform.position = Vector3.MoveTowards(cb.obj.transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(cb.obj.transform.position, target) < 0.05f)
            {
                cb.obj.transform.position = target;

                // Box has cleared the spawn node — allow the next one to spawn
                if (cb.targetNodeIndex == 1)
                    spawnNodeOccupied = false;

                if (cb.targetNodeIndex < pathNodes.Count - 1)
                {
                    cb.targetNodeIndex++;
                }
                else if (cb.assignedSlot == -1)
                {
                    // End of conveyor — find a free slot
                    for (int s = 0; s < slotOccupied.Length; s++)
                    {
                        if (!slotOccupied[s])
                        {
                            slotOccupied[s] = true;
                            cb.assignedSlot = s;
                            cb.obj.transform.position = slotNodes[s].position;
                            break;
                        }
                    }
                }
            }
        }
    }
}
