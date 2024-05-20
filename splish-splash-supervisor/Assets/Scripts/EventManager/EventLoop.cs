using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//EventLoop class used to spawn events based of probabilities
//Also used to manage NPC spawning and time tracking
public class EventLoop : MonoBehaviour
{
    public Timer timer;
    public EventManager eventManager;
    public NPCManager npcManager;
    public EventData eventData;
    private Text clock;
    private Dictionary<EventType, float> eventProbabilitesDict;
    private NPCLine icecreamLine;
    private GameObject icecreamStand;

    void Start()
    {
        clock = GameObject.Find("Clock").GetComponent<Text>();
        eventProbabilitesDict = new Dictionary<EventType, float>();

        //For day cycles and spawning NPCs
        timer.StartTimer();

        //Probabilty dictionary
        //eventProbabilitesDict.Add(EventType.Drowning, eventData.drowningProbability);
        eventProbabilitesDict.Add(EventType.Running, eventData.runningProbability);

        //Will remove this block later (Will be replaced by SpawnNPCs)
        npcManager.StartSpawning();

        StartCoroutine(RunProbabilites());
        StartCoroutine(ManageSatisfaction());
        StartCoroutine(UpdateClock());
        npcManager.StartIceCreamStand();
    }

    IEnumerator UpdateClock()
    {
        while (true)
        {
            clock.text = timer.GetCurrentTime();
            yield return new WaitForSeconds(1);
        }
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
            NPC npc = npcManager.GetRandomNPC();
            switch (npc.GetStatus())
            {
                case NPCStatus.Swimming:
                    StartRandomPoolEvent(npc);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(eventData.reRollRate);
        }
    }
    private void StartRandomPoolEvent(NPC npc)
    {
        foreach (KeyValuePair<EventType, float> entry in eventProbabilitesDict)
        {
            float rand = Random.Range(0f, 100f);

            if (npc != null && npc.GetStatus() != NPCStatus.Travelling && npc.GetIsEventOccuring() == false)
            {
                if (rand <= entry.Value)
                {
                    if (entry.Key == EventType.Running)
                    {
                        npc.SetNewTargetLocation(Location.RunningArea);
                    }
                    else
                    {
                        IEvent e = CreateEvent(entry.Key, npc);
                        eventManager.TriggerEvent(e);
                    }
                }
            }
        }
    }

    IEvent CreateEvent(EventType type, NPC npc)
    {
        switch (type)
        {
            case EventType.Drowning:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.drowningDuration, eventData.drowningDamageRate, type, eventData.drowningSatisfactionDropRate, eventData.drowningSize);
            case EventType.Shitting:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.shittingDuration, eventData.shittingDamageRate, type, eventData.shittingSatisfactionDropRate, eventData.shittingSize);
            case EventType.Pissing:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.pissingDuration, eventData.pissingDamageRate, type, eventData.pissingSatisfactionDropRate, eventData.pissingSize);
            case EventType.Running:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.runningDuration, eventData.runningDamageRate, type, eventData.runningSatisfactionDropRate, eventData.runningSize);
            case EventType.OverHeating:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.overHeatingDuration, eventData.overHeatingDamageRate, type, eventData.overHeatingSatisfactionDropRate, eventData.overHeatingSize);
            case EventType.Hysteria:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.hysteriaDuration, eventData.hysteriaDamageRate, type, eventData.hysteriaSatisfactionDropRate, eventData.hysteriaSize);
            default:
                return null;
        }
    }

    public IEvent CreateEventNPC(EventType type, NPC npc)
    {
        switch (type)
        {
            case EventType.Drowning:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.drowningDuration, eventData.drowningDamageRate, type, eventData.drowningSatisfactionDropRate, eventData.drowningSize);
            case EventType.Shitting:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.shittingDuration, eventData.shittingDamageRate, type, eventData.shittingSatisfactionDropRate, eventData.shittingSize);
            case EventType.Pissing:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.pissingDuration, eventData.pissingDamageRate, type, eventData.pissingSatisfactionDropRate, eventData.pissingSize);
            case EventType.Running:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.runningDuration, eventData.runningDamageRate, type, eventData.runningSatisfactionDropRate, eventData.runningSize);
            case EventType.OverHeating:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.overHeatingDuration, eventData.overHeatingDamageRate, type, eventData.overHeatingSatisfactionDropRate, eventData.overHeatingSize);
            case EventType.Hysteria:
                return new GenericEvent(timer.GetExactTime(), npc, eventData.hysteriaDuration, eventData.hysteriaDamageRate, type, eventData.hysteriaSatisfactionDropRate, eventData.hysteriaSize);
            default:
                return null;
        }
    }
}
