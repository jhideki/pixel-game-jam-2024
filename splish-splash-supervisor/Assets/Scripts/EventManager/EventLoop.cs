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
public class EventLoop : MonoBehaviour
{
    public Timer timer;
    public EventManager eventManager;
    public NPCManager npcManager;
    public EventData eventData;
    private Dictionary<EventType, float> eventProbabilitesDict;

    // Start is called before the first frame update
    void Start()
    {
        eventProbabilitesDict = new Dictionary<EventType, float>();
        timer.StartTimer();

        //Probabilty dictionary
        eventProbabilitesDict.Add(EventType.Drowning, eventData.drowningProbability);
        eventProbabilitesDict.Add(EventType.Shitting, eventData.shittingProbalilty);
        eventProbabilitesDict.Add(EventType.Pissing, eventData.pissingProbalility);
        eventProbabilitesDict.Add(EventType.Running, eventData.runningProbability);
        eventProbabilitesDict.Add(EventType.OverHeating, eventData.overHeatingProbabilty);
        eventProbabilitesDict.Add(EventType.Hysteria, eventData.hysteriaProbability);

        for (int i = 0; i < 10; i++)
        {
            npcManager.Spawn(new Vector2Int(Random.Range(-10, 10), Random.Range(-10, 10)));
        }

        StartCoroutine(RunProbabilites());
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
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.drowningDuration, eventData.drowningDamageRate, type, eventData.drowningSatisfactionDropRate);
            case EventType.Shitting:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.shittingDuration, eventData.shittingDamangeRate, type, eventData.shittingSatisfactionDropRate);
            case EventType.Pissing:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.pissingDuration, eventData.pissingDamageRate, type, eventData.pissingSatisfactionDropRate);
            case EventType.Running:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.runningDuration, eventData.runningDamageRate, type, eventData.runningSatisfactionDropRate);
            case EventType.OverHeating:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.overHeatingProbabilty, eventData.overHeatingDamageRate, type, eventData.overHeatingSatisfactionDropRate);
            case EventType.Hysteria:
                return new GenericEvent(timer.GetCurrentTime(), npc, eventData.hysteriaDuration, eventData.hysteriaDamageRate, type, eventData.hysteriaSatisfactionDropRate);
            default:
                return null;
        }
    }
}
