using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public NPCManager npcManager;
    private List<IEvent> activeEvents = new List<IEvent>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void TriggerEvent(IEvent newEvent)
    {
        activeEvents.Add(newEvent);
        StartCoroutine(RunEvent(newEvent));
    }

    private IEnumerator RunEvent(IEvent newEvent)
    {
        Debug.Log(newEvent.nPC.GetComponent<NPC>().GetName() + " started " + newEvent.Type);

        NPC npc = newEvent.nPC.GetComponent<NPC>();
        float elapsedTime = 0f;
        while (elapsedTime < newEvent.Duration && newEvent.isActive)
        {
            elapsedTime += 1f;

            //target NPC associated with event
            npc.DealDamage(newEvent.DamageRate);
            npc.LowerSatisfaction(newEvent.SatisfactionDropRate);

            //target overall satisfaction
            npcManager.dealSatisfactionDamage(newEvent.SatisfactionDropRate);

            //Wait for one second before continuing
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Event " + newEvent.Type + " finished");
        Debug.Log("NPC " + newEvent.nPC.GetComponent<NPC>().GetName() + " now has " + newEvent.nPC.GetComponent<NPC>().GetHealth() + "health and " + newEvent.nPC.GetComponent<NPC>().GetSatisfaction() + " satisfaction");

        //cleanup
        activeEvents.Remove(newEvent);
        if (npc.GetStatus() == NPCStatus.Dead)
        {
            npcManager.RemoveNPC(npc.gameObject);
        }
    }
}