using System.Collections.Generic;
using UnityEngine;

public enum NPCStatus
{
    Swimming,
    Drowning,
    Shitting,
    Pissing,
    Overheating,
    Running,
    Idle,
    Hysteria,
    Dead,
}

public class NPCManager : MonoBehaviour
{
    private List<GameObject> nPCs = new List<GameObject>();
    private float startingSatisfaction = 0f;
    private float currentSatisfaction = 0f;
    public GameObject NPCPrefab;
    public EventData eventData;

    private Vector3 spawnLocation = new Vector3(12, -8, 0); // Fixed spawn location
    public float spawnDelay = 5.0f; // Delay time in seconds between spawns

    public IEnumerator Spawn()
    {
        while (true)
        {

            //GameObject nPCInsatnce = Instantiate(NPCPrefab, new Vector3(location.x, location.y, 0), Quaternion.identity);
            GameObject nPCInsatnce = Instantiate(NPCPrefab, spawnLocation, Quaternion.identity);
            NPC npc = nPCInsatnce.GetComponent<NPC>();

            //npc.GetPath();

            startingSatisfaction += npc.GetSatisfaction();
            currentSatisfaction += npc.GetSatisfaction();
            nPCs.Add(nPCInsatnce);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    // Method to start the spawning coroutine
    public void StartSpawning()
    {
        StartCoroutine(Spawn());
    }

    public NPC GetRandomNPC()
    {
        return nPCs[Random.Range(0, nPCs.Count)].GetComponent<NPC>();
    }

    public void IncreaseSatisfaction()
    {
        foreach (GameObject npc in nPCs)
        {
            NPC n = npc.GetComponent<NPC>();
            if (n.GetSatisfaction() < n.GetMaxSatisfaction())
            {
                if (n.GetSatisfaction() + eventData.satisfactionIncreaseAmount < n.GetMaxSatisfaction())
                {
                    n.IncreaseSatisfaction(eventData.satisfactionIncreaseAmount);
                    currentSatisfaction += eventData.satisfactionIncreaseAmount;
                }
                else if (eventData.satisfactionIncreaseAmount > n.GetSatisfaction() - n.GetMaxSatisfaction())
                {
                    n.IncreaseSatisfaction(eventData.satisfactionIncreaseAmount - (n.GetSatisfaction() - n.GetMaxSatisfaction()));
                    currentSatisfaction += n.GetSatisfaction() - n.GetMaxSatisfaction();
                }
            }
        }
    }

    //TDO - add a method to deal damage to all NPCs
    public void DealSatisfactionDamage(float amount, NPC npc)
    {
        npc.LowerSatisfaction(amount);
        currentSatisfaction -= amount;
    }

    public void RemoveNPC(GameObject npc)
    {
        if (nPCs.Contains(npc))
        {
            nPCs.Remove(npc);
            Destroy(npc);
        }
    }

}
