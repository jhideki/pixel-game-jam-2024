using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public NPCManager npcManager;
    public List<IEvent> activeEvents = new List<IEvent>();
    public EventData eventData;
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

    public void checkCollisions(Transform player)
    {
        foreach (IEvent e in activeEvents)
        {

            NPC npc = e.nPC.GetComponent<NPC>();
            if (e.isActive)
            {
                if (player.position.x > e.location.x - eventData.eventRadius && player.position.x < e.location.x + eventData.eventRadius &&
                    player.position.y > e.location.y - eventData.eventRadius && player.position.y < e.location.y + eventData.eventRadius)
                {
                    EndEvent(e);
                }
            }

        }


    }

    public void EndEvent(IEvent e)
    {
        e.isActive = false;
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