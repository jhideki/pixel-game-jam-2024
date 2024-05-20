using System.Collections.Generic;
using System.Collections;
using UnityEngine;



public class NPCManager : MonoBehaviour
{
    public List<GameObject> npcVariants = new List<GameObject>();
    private List<GameObject> nPCs = new List<GameObject>();
    private float startingSatisfaction = 0f;
    private float currentSatisfaction = 0f;
    public EventData eventData;

    public HashSet<Vector3> occupiedHottubCoordinates = new HashSet<Vector3>();


    public Vector3 spawnLocation = new Vector3(12, -8, 0); // Fixed spawn location
    public float spawnDelay = 100f; // Delay time in seconds between spawns
    private GameObject icecreamStand;
    private NPCLine icecreamLine;
    public float icecreamStandDelay = 5f;

    public IEnumerator Spawn()
    {
        while (true)
        {
            GameObject npcPrefab = npcVariants[Random.Range(0, npcVariants.Count)];
            GameObject nPCInsatnce = Instantiate(npcPrefab, spawnLocation, Quaternion.identity);
            NPC npc = nPCInsatnce.GetComponent<NPC>();
            npc.InitializeNPC();

            startingSatisfaction += npc.GetSatisfaction();
            currentSatisfaction += npc.GetSatisfaction();
            nPCs.Add(nPCInsatnce);

            yield return new WaitForSeconds(spawnDelay);
        }
    }
    public void StartIceCreamStand()
    {
        //Icecream ;)
        icecreamStand = GameObject.Find("IcecreamStand");
        icecreamLine = icecreamStand.GetComponent<NPCLine>();
        StartCoroutine(SendToIceCreamStand());
    }

    IEnumerator SendToIceCreamStand()
    {
        while (true)
        {
            NPC npc = GetRandomNPC();
            if (!npc.GetIsEventOccuring() && npc.GetStatus() == NPCStatus.Swimming || npc.GetStatus() == NPCStatus.Hottub && !icecreamLine.IsFull())
            {
                icecreamLine.EnqueueLine(npc.gameObject);
                npc.SetNewTargetLocationCoords(icecreamLine.GetNextLocation(), Location.IcecreamStand);
                npc.SetIcecreamPosition(icecreamLine.GetNextLocation());

            }
            yield return new WaitForSeconds(icecreamStandDelay);
        }

    }

    // Method to start the spawning coroutine
    public void StartSpawning()
    {
        StartCoroutine(Spawn());
    }

    public NPC GetRandomNPC()
    {
        if (nPCs.Count == 0)
        {
            return null;
        }
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
