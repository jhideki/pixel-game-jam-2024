using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEvent
{
    EventType Type { get; }
    float SatisfactionDropRate { get; }
    float StartTime { set; get; }
    float Duration { get; set; }
    int DamageRate { get; set; }
    NPC nPC { get; set; }
    bool isActive { get; set; }
    Vector2Int location { get; set; }
}

public abstract class EventBase : IEvent
{
    public EventType Type { get; set; }
    //Rate at which satisfaction decreases per second
    public float SatisfactionDropRate { get; set; }
    public float StartTime { set; get; }
    public NPC nPC { get; set; }

    public int DamageRate { get; set; }
    public float Duration { get; set; }
    public abstract bool isActive { get; set; }
    public Vector2Int location { get; set; }

}

public class GenericEvent : EventBase
{
    public GenericEvent(float startTime, NPC nPC, float duration, int damageRate, EventType type, float satisfactionDropRate)
    {
        Type = type;
        SatisfactionDropRate = satisfactionDropRate;
        StartTime = startTime;
        this.nPC = nPC;
        DamageRate = damageRate;
        Duration = duration;
        isActive = true;
        location = nPC.GetLocation();
    }
    public override bool isActive { get; set; }
}

public class NonNPCEvent : EventBase
{

    public NonNPCEvent(float startTime, NPC nPC, float duration, int damageRate, EventType type, float satisfactionDropRate, Vector2Int location)
    {
        Type = type;
        SatisfactionDropRate = satisfactionDropRate;
        StartTime = startTime;
        this.nPC = nPC;
        DamageRate = damageRate;
        Duration = duration;
        isActive = true;
        this.location = location;
    }
    public override bool isActive { get; set; }
}
