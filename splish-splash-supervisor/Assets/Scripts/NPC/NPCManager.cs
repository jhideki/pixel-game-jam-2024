using System.Collections.Generic;
using System.Collections;
using UnityEngine;



public class NPCManager : MonoBehaviour
{
    public List<GameObject> npcVariants = new List<GameObject>();
    private List<GameObject> nPCs = new List<GameObject>();
    private List<GameObject> icecreamLine = new List<GameObject>();
    private float startingSatisfaction = 0f;
    private float currentSatisfaction = 0f;
    public EventData eventData;

    public Vector3 spawnLocation = new Vector3(12, -8, 0); // Fixed spawn location
    public float spawnDelay = 4f; // Delay time in seconds between spawns

    public IEnumerator Spawn()
    {
        while (true)
        {
            GameObject npcPrefab = npcVariants[Random.Range(0, npcVariants.Count)];
            GameObject nPCInsatnce = Instantiate(npcPrefab, spawnLocation, Quaternion.identity);
            NPC npc = nPCInsatnce.GetComponent<NPC>();


            startingSatisfaction += npc.GetSatisfaction();
            currentSatisfaction += npc.GetSatisfaction();
            nPCs.Add(nPCInsatnce);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SendToIceCreamStand()
    {
        foreach (var npcObject in nPCs)
        {
            NPC npc = npcObject.GetComponent<NPC>();
            if (!npc.GetIsEventOccuring() && npc.GetStatus() != NPCStatus.Travelling && npc.GetStatus() != NPCStatus.IcreamLine)
            {
                //npc.SetNewTargetLocation(icecreamLine.GetNextPosition());
                icecreamLine.Add(npcObject);
                break;
            }
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
