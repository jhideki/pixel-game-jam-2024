using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventLoop : MonoBehaviour
{
    public Timer timer;
    public EventManager eventManager;
    public NPCManager npcManager;
    public EventData eventData;
    public enum EventType
    {
        Drowning,
        Shitting
    }
    private Dictionary<EventType, float> eventProbabilitesDict;

    // Start is called before the first frame update
    void Start()
    {
        eventProbabilitesDict = new Dictionary<EventType, float>();
        timer.StartTimer();
        eventProbabilitesDict.Add(EventType.Drowning, eventData.drowningProbability);
        eventProbabilitesDict.Add(EventType.Shitting, eventData.shittingProbalilty);
        for (int i = 0; i < 10; i++)
        {
            npcManager.Spawn(new Vector2Int(Random.Range(0, 100), Random.Range(0, 100)));
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
                return new DrowningEvent(timer.GetCurrentTime(), npc, eventData.drowningDuration, eventData.drowningDamageRate);
            case EventType.Shitting:
                return new ShittingEvent(timer.GetCurrentTime(), npc, eventData.shittingDuration, eventData.shittingDamangeRate);
            default:
                return null;
        }
    }
}
