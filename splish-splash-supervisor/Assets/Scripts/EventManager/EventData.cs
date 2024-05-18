using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "EventData/Event Data", order = 1)]
public class EventData : ScriptableObject
{

    [Header("General")]
    public float reRollRate = 1f;
    public int eventRadius = 2;
    public float satisfactionIncreaseAmount = 1f;
    public float satisfactionIncreaseTime = 1f;

    [Header("Shitting")]
    public float shittingProbability = 10f;
    public float shittingSatisfactionDropRate = 1f;
    public int shittingDamageRate = 1;
    public float shittingDuration = 5f;
    public int shittingSize = 1;

    [Header("Pissing")]
    public int pissingProbability = 10;
    public float pissingSatisfactionDropRate = 1f;
    public int pissingDamageRate = 10;
    public float pissingDuration = 10f;

    public int pissingSize = 1;

    [Header("Drowning")]
    public float drowningProbability = 10f;
    public float drowningSatisfactionDropRate = 1f;
    public int drowningDamageRate = 10;
    public float drowningDuration = 10f;
    public int drowningSize = 1;

    [Header("Overheating")]
    public int overHeatingProbability = 0;
    public float overHeatingSatisfactionDropRate = 50f;
    public int overHeatingDamageRate = 20;
    public float overHeatingDuration = 100000000f;
    public int overHeatingSize = 1;
    public int preheatDuration = 10;

    [Header("Hysteria")]
    public int hysteriaProbability = 10;
    public float hysteriaSatisfactionDropRate = 1f;
    public int hysteriaDamageRate = 10;
    public float hysteriaDuration = 10f;
    public int hysteriaSize = 1;

    [Header("Running")]
    public int runningProbability = 10;
    public float runningSatisfactionDropRate = 1f;
    public int runningDamageRate = 10;
    public float runningDuration = 10f;
    public int runningSize = 1;

}
