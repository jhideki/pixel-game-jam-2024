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
    [Header("Satisfaction")]
    public float shittingSatisfactionDropRate = 1f;
    public float pissingSatisfactionDropRate = 1f;
    public float drowningSatisfactionDropRate = 1f;
    public float overHeatingSatisfactionDropRate = 1f;
    public float hysteriaSatisfactionDropRate = 1f;
    public float runningSatisfactionDropRate = 1f;

    [Header("Probabilities")]
    public float shittingProbalilty = 10f;
    public float drowningProbability = 10f;
    public int pissingProbalility = 10;
    public int runningProbability = 10;
    public int hysteriaProbability = 10;
    public int overHeatingProbabilty = 10;

    [Header("Damage")]

    public int drowningDamageRate = 10;

    public int pissingDamageRate = 10;

    public int runningDamageRate = 10;
    public int hysteriaDamageRate = 10;

    public int shittingDamangeRate = 1;
    public int overHeatingDamageRate = 10;
    [Header("Duration")]
    public float overHeatingDuration = 10f;
    public float runningDuration = 10f;
    public float pissingDuration = 10f;
    public float hysteriaDuration = 10f;
    public float drowningDuration = 10f;
    public float shittingDuration = 5f;
}
