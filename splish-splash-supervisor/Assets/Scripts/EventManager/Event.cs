using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public interface IEvent
{
    string Name { get; }
    float SatisfactionDropRate { get; }
    float StartTime { set; get; }
    float Duration { get; set; }
    int DamageRate { get; set; }
    NPC nPC { get; set; }

}

public abstract class EventBase : IEvent
{
    public abstract string Name { get; }
    //Rate at which satisfaction decreases per second
    public abstract float SatisfactionDropRate { get; }
    public float StartTime { set; get; }
    public NPC nPC { get; set; }

    public int DamageRate { get; set; }
    public float Duration { get; set; }
}

public class DrowningEvent : EventBase
{
    public override string Name => "Drowning";
    public override float SatisfactionDropRate => 10f;
    public DrowningEvent(float startTime, NPC nPC, float duration, int damageRate)
    {
        StartTime = startTime;
        this.nPC = nPC;
        DamageRate = damageRate;
        Duration = duration;
    }
}

public class ShittingEvent : EventBase
{
    public override string Name => "Shitting";
    public override float SatisfactionDropRate => 10f;
    public ShittingEvent(float startTime, NPC nPC, float duration, int damageRate)
    {
        StartTime = startTime;
        this.nPC = nPC;
        DamageRate = damageRate;
        Duration = duration;
    }
}