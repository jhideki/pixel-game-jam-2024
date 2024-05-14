using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EventType
{
    Drowning,
    Shitting,
    Pissing,
    OverHeating,
    Running,
    Hysteria,
}
//EventLoop class used to spawn events based of probabilities
//Also used to manage NPC spawning and time tracking
public class EventLoop : MonoBehaviour
{
    public Timer timer;
    public EventManager eventManager;
    public NPCManager npcManager;
    public EventData eventData;
    private Dictionary<EventType, float> eventProbabilitesDict;

    void Start()
    {
        eventProbabilitesDict = new Dictionary<EventType, float>();

        //For day cycles and spawning NPCs
        timer.StartTimer();

        //Probabilty dictionary
        eventProbabilitesDict.Add(EventType.Drowning, eventData.drowningProbability);
        eventProbabilitesDict.Add(EventType.Shitting, eventData.shittingProbability);
        eventProbabilitesDict.Add(EventType.Pissing, eventData.pissingProbability);
        eventProbabilitesDict.Add(EventType.Running, eventData.runningProbability);
        eventProbabilitesDict.Add(EventType.OverHeating, eventData.overHeatingProbability);
        eventProbabilitesDict.Add(EventType.Hysteria, eventData.hysteriaProbability);

        //Will remove this block later (Will be replaced by SpawnNPCs)
        for (int i = 0; i < 10; i++)
        {
            npcManager.Spawn(new Vector2Int(Random.Range(-10, 10), Random.Range(-10, 10)));
        }
        //End block 

        StartCoroutine(RunProbabilites());
        StartCoroutine(ManageSatisfaction());
    }

    void SpawnNPCs()
    {
        //TODO: spawn NPCs based on time of day (use timer and other metrics)
        // npcManager.Spawn(<SOME_VECTOR2_INT_SPAWN_LOCATION>);
    }

    void Update()
    {
        SpawnNPCs();


    }

    IEnumerator ManageSatisfaction()
    {
        while (true)
        {
            npcManager.IncreaseSatisfaction();
            yield return new WaitForSeconds(eventData.satisfactionIncreaseTime);
        }
    }

    IEnumerator RunProbabilites()
    {
        while (true)
        {
            foreach (KeyValuePair<EventType, float> entry in eventProbabilitesDict)
            {
                float rand = Random.Range(0f, 100f);
                if (rand <= entry.Value)
                {
                    IEvent e = CreateEvent(entry.Key);
                    eventManager.TriggerEvent(e);
                }
            }
            yield return new WaitForSeconds(eventData.reRollRate);
        }
    }

    IEvent CreateEvent(EventType type)
    {
        NPC npc = npcManager.GetRandomNPC();
        switch (type)
        {
            case EventType.Drowning:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.drowningDuration, eventData.drowningDamageRate, type, eventData.drowningSatisfactionDropRate, eventData.drowningSize);
            case EventType.Shitting:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.shittingDuration, eventData.shittingDamageRate, type, eventData.shittingSatisfactionDropRate, eventData.shittingSize);
            case EventType.Pissing:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.pissingDuration, eventData.pissingDamageRate, type, eventData.pissingSatisfactionDropRate, eventData.pissingSize);
            case EventType.Running:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.runningDuration, eventData.runningDamageRate, type, eventData.runningSatisfactionDropRate, eventData.runningSize);
            case EventType.OverHeating:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.overHeatingDuration, eventData.overHeatingDamageRate, type, eventData.overHeatingSatisfactionDropRate, eventData.overHeatingSize);
            case EventType.Hysteria:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.hysteriaDuration, eventData.hysteriaDamageRate, type, eventData.hysteriaSatisfactionDropRate, eventData.hysteriaSize);
            default:
                return null;
        }
    }
}
