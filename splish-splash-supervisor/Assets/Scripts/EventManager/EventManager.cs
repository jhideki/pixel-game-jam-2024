using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
using System;
using UnityEngine.SocialPlatforms.Impl;

//EventManager class used to spawn events keep track of active events
public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public GameObject npcDeath;
    public GameObject eventPrefab;
    public NPCManager npcManager;
    public GameObject eventManager;
    public List<GameObject> activeEvents = new List<GameObject>();
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

    public void EndEvent(GameObject eventObject)
    {
        IEvent e = eventObject.GetComponent<EventObject>().GetEvent();
        NPC npc = e.nPC;
        //TODO: call npc functions to change status of  npc.
        //E.g., npc.setStatus(travelling) + npc.setNewTargetLocation()
        switch (e.Type)
        {
            case EventType.Drowning:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Shitting:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Pissing:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Running:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.OverHeating:
                npc.SetNewTargetLocation(Location.Pool);
                break;
            case EventType.Hysteria:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            default:
                break;
        }

        e.isActive = false;
    }

    public void EndEventIEvent(IEvent e)
    {
        NPC npc = e.nPC;
        //TODO: call npc functions to change status of  npc.
        //E.g., npc.setStatus(travelling) + npc.setNewTargetLocation()
        switch (e.Type)
        {
            case EventType.Drowning:
                npc.SetNewTargetLocation(Location.Pool);
                break;
            case EventType.Shitting:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Pissing:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Running:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.OverHeating:
                npc.SetNewTargetLocation(Location.Pool);
                break;
            case EventType.Hysteria:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            default:
                break;
        }

        e.isActive = false;
    }

    public void EndEventLose(IEvent e)
    {
        NPC npc = e.nPC;
        //TODO: call npc functions to change status of  npc.
        //E.g., npc.setStatus(travelling) + npc.setNewTargetLocation()
        switch (e.Type)
        {
            case EventType.Drowning:
                npc.SetStatus(NPCStatus.Dead);
                break;
            case EventType.Shitting:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Pissing:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.Running:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            case EventType.OverHeating:
                npc.SetNewTargetLocation(Location.Pool);
                break;
            case EventType.Hysteria:
                npc.SetNewTargetLocation(Location.Hottub);
                break;
            default:
                break;
        }

        e.isActive = false;
    }

    public GameObject TriggerEvent(IEvent newEvent)
    {
        GameObject eventObject = Instantiate(eventPrefab, new Vector3(newEvent.location.x, newEvent.location.y, 0), Quaternion.identity);
        eventObject.GetComponent<EventObject>().SetEvent(newEvent);
        activeEvents.Add(eventObject);
        StartCoroutine(RunEvent(eventObject));
        return eventObject;
    }

    private IEnumerator RunEvent(GameObject eventObject)
    {
        IEvent newEvent = eventObject.GetComponent<EventObject>().GetEvent();
        Debug.Log(newEvent.nPC.GetComponent<NPC>().GetName() + " started " + newEvent.Type);

        NPC npc = newEvent.nPC.GetComponent<NPC>();

        if (newEvent.Type != EventType.Running)
        {
            npc.SetStatus(NPCStatus.EventOccuring);
        }
        NPCStatus previousStatus = npc.GetStatus();
        float elapsedTime = 0f;
        while (elapsedTime < newEvent.Duration && newEvent.isActive && npc.GetStatus() != NPCStatus.Dead)
        {
            elapsedTime += 1f;

            //target NPC associated with event
            npc.DealDamage(newEvent.DamageRate);

            //target NPC and Overall satisfaction
            npcManager.DealSatisfactionDamage(newEvent.SatisfactionDropRate, npc);

            //Wait for one second before continuing
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Event " + newEvent.Type + " finished");
        Debug.Log("NPC " + newEvent.nPC.GetComponent<NPC>().GetName() + " now has " + newEvent.nPC.GetComponent<NPC>().GetHealth() + "health and " + newEvent.nPC.GetComponent<NPC>().GetSatisfaction() + " satisfaction");

        //cleanup
        activeEvents.Remove(eventObject);
        Destroy(eventObject);
        if (npc.GetStatus() == NPCStatus.Dead)
        {
            GameObject death = Instantiate(npcDeath, npc.transform.position, Quaternion.identity);
            npcManager.DealSatisfactionDamage(100f, npc);
            npcManager.RemoveNPC(npc.gameObject);
            npcManager.DeathPenalty();
        }
        else if (npc.GetStatus() != NPCStatus.Travelling)
        {
            npc.SetStatus(previousStatus);
        }
    }
}