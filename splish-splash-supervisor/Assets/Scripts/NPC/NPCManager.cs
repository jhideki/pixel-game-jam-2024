using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void Spawn(Vector2Int location)
    {
        GameObject nPCInsatnce = Instantiate(NPCPrefab, new Vector3(location.x, location.y, 0), Quaternion.identity);
        NPC npc = nPCInsatnce.GetComponent<NPC>();
        startingSatisfaction += npc.GetSatisfaction();
        currentSatisfaction += npc.GetSatisfaction();
        nPCs.Add(nPCInsatnce);
    }

    public NPC GetRandomNPC()
    {
        return nPCs[Random.Range(0, nPCs.Count)].GetComponent<NPC>();
    }

    //TDO - add a method to deal damage to all NPCs
    public void dealSatisfactionDamage(float amount)
    {
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
