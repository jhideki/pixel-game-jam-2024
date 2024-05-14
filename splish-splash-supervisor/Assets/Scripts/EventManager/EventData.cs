using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "EventData/Event Data", order = 1)]
public class EventData : ScriptableObject
{
    public float reRollRate = 1f;
    public float shittingProbalilty = 10f;
    public float drowningProbability = 10f;
    public int drowningDamageRate = 10;
    public int shittingDamangeRate = 1;
    public float shittingDuration = 5f;
    public float drowningDuration = 3f;
}
