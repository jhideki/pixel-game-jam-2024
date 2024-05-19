using System.Collections;
using System.Collections.Generic;
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

public enum Direction
{
    North,
    South,
    East,
    West,
}

public enum Location
{
    Hottub,
    Pool,
    IcecreamStand,
    None,
}

public enum NPCStatus
{
    Swimming,
    Hottub,
    Travelling,
    Idle,
    Dead,
    IcreamLine,
    EventOccuring,
}

public enum CoroutineType
{
    Swimming,
    Travelling,
    Hottub,
    EventOccuring,
}
