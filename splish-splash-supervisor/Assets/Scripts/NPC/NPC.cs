using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public class NPC : MonoBehaviour
{
    //Constant parameters
    public NPCParameters parameters;
    public EventManager eventManager;
    public EventData eventData;
    public EventLoop eventLoop;

    //NPC variables
    private int health;
    private float satisfaction;
    private float startingSatisfaction;
    private string name;
    private NPCStatus status;
    private Location targetLocation;

    private NPCManager npcManager;
    private bool isEventOccuring;
    private bool isStatusRoutineRunning;
    private Coroutine currentCoroutine;
    private CoroutineType coroutineType;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private Queue<Vector3> pathQueue = new Queue<Vector3>();
    private Vector3 hotTubPosition;
    private Vector2Int icecreamPosition;

    private static List<Vector3> hottubCoordinates = new List<Vector3>
    {
        new Vector3(12, -7, 0),
        new Vector3(11, -7, 0),
        new Vector3(10, -7, 0),
        new Vector3(10, -6, 0),
        new Vector3(11, -6, 0),
        new Vector3(12, -6, 0),
        new Vector3(12, -5, 0),
        new Vector3(11, -5, 0),
        new Vector3(10, -5, 0),
        // Add more coordinates as needed
    };

    public void InitializeNPC()
    {

        rb = GetComponent<Rigidbody2D>();
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventLoop = GameObject.Find("EventManager").GetComponent<EventLoop>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        name = parameters.names[Random.Range(0, parameters.names.Length)];
        health = Random.Range(parameters.minHealth, parameters.maxHealth);
        satisfaction = Random.Range(parameters.minSatisfaction, parameters.maxSatisfaction);
        startingSatisfaction = satisfaction;
        Debug.Log("Created " + name + " with health " + health + " and satisfaction " + satisfaction);
        SetSpawnTargetLocation();
    }

    void Update()
    {
        switch (status)
        {
            case NPCStatus.Travelling:
                if (currentCoroutine != null && coroutineType != CoroutineType.Travelling)
                {
                    RemoveHottub();
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(MoveTowardsTarget());
                }
                else if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(MoveTowardsTarget());
                }
                break;
            case NPCStatus.Swimming:
                if (currentCoroutine != null && coroutineType != CoroutineType.Swimming)
                {
                    RemoveHottub();
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(Swim());
                }
                else if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(Swim());
                }
                break;
            case NPCStatus.Hottub:
                if (currentCoroutine != null && coroutineType != CoroutineType.Hottub)
                {
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(Hottub());
                }
                else if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(Hottub());
                }
                break;
            case NPCStatus.EventOccuring:
                if (currentCoroutine != null && coroutineType != CoroutineType.EventOccuring)
                {
                    RemoveHottub();
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(EventOccuring());
                }
                else if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(EventOccuring());
                }
                break;
            case NPCStatus.IcreamLine:
                if (currentCoroutine != null && coroutineType != CoroutineType.IcecreamLine)
                {
                    RemoveHottub();
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(IcecreamLine());
                }
                else if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(IcecreamLine());
                }
                break;
            case NPCStatus.Running:
                if (currentCoroutine != null && coroutineType != CoroutineType.Running)
                {
                    RemoveHottub();
                    StopCoroutine(currentCoroutine);
                    currentCoroutine = StartCoroutine(Running());
                }
                else if (currentCoroutine == null)
                {
                    currentCoroutine = StartCoroutine(Running());
                }
                break;
        }
    }

    IEnumerator Running()
    {
        coroutineType = CoroutineType.Running;

        IEvent running = eventLoop.CreateEventNPC(EventType.Running, this);
        eventManager.TriggerEvent(running);
        while (status == NPCStatus.Running)
        {
            Direction randomDirection = (Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length);
            Vector3 velocity = new Vector3(0f, 0f, 0f);
            switch (randomDirection)
            {
                case Direction.North:
                    velocity.y = parameters.runVelocity;
                    break;
                case Direction.South:
                    velocity.y = -parameters.runVelocity;
                    break;
                case Direction.West:
                    velocity.x = -parameters.runVelocity;
                    break;
                case Direction.East:
                    velocity.x = parameters.runVelocity;
                    break;
            }
            rb.velocity = velocity;
            yield return new WaitForSeconds(Random.Range(parameters.changeDirectionIntervalMin, parameters.changeDirectionIntervalMax));
        }
    }
    private void RemoveHottub()
    {
        if (coroutineType == CoroutineType.Hottub)
        {
            npcManager.occupiedHottubCoordinates.Remove(hotTubPosition);
            hotTubPosition = Vector3.zero;
        }
    }

    IEnumerator IcecreamLine()
    {
        coroutineType = CoroutineType.IcecreamLine;
        //deal 2 damage every 5 seconds
        while (status == NPCStatus.IcreamLine)
        {
            npcManager.DealSatisfactionDamage(2f, this);
            yield return new WaitForSeconds(5f);
        }
    }
    IEnumerator EventOccuring()
    {
        coroutineType = CoroutineType.EventOccuring;
        rb.velocity = Vector2.zero;
        while (status == NPCStatus.EventOccuring)
        {
            yield return null;
        }
        isStatusRoutineRunning = false;
    }

    IEnumerator MoveTowardsTarget()
    {
        coroutineType = CoroutineType.Travelling;
        rb.velocity = Vector2.zero;
        //Check if queue is empty and if there is a target location, if not set new target and move towards target
        while (pathQueue.Count > 0 && targetLocation != Location.None && status == NPCStatus.Travelling)
        {
            Vector3 targetPosition = pathQueue.Dequeue();

            while (transform.position != targetPosition && status == NPCStatus.Travelling)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, parameters.moveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        //We have now exhausted all of the paths in the queue and have reached our destination
        switch (targetLocation)
        {
            case Location.Hottub:
                status = NPCStatus.Hottub;
                break;
            case Location.Pool:
                status = NPCStatus.Swimming;
                break;
            case Location.IcecreamStand:
                status = NPCStatus.IcreamLine;
                break;
            case Location.RunningArea:
                status = NPCStatus.Running;
                break;
        }

        //Set target location to none
        targetLocation = Location.None;
    }


    IEnumerator Swim()
    {
        coroutineType = CoroutineType.Swimming;
        while (status == NPCStatus.Swimming)
        {
            Direction randomDirection = (Direction)Random.Range(0, System.Enum.GetValues(typeof(Direction)).Length);
            Vector3 velocity = new Vector3(0f, 0f, 0f);
            switch (randomDirection)
            {
                case Direction.North:
                    velocity.y = parameters.swimVelocity;
                    break;
                case Direction.South:
                    velocity.y = -parameters.swimVelocity;
                    break;
                case Direction.West:
                    velocity.x = -parameters.swimVelocity;
                    break;
                case Direction.East:
                    velocity.x = parameters.swimVelocity;
                    break;
            }
            rb.velocity = velocity;
            yield return new WaitForSeconds(Random.Range(parameters.changeDirectionIntervalMin, parameters.changeDirectionIntervalMax));
        }

        isStatusRoutineRunning = false;
    }


    IEnumerator Hottub()
    {
        coroutineType = CoroutineType.Hottub;
        bool assignedToHottub = false;

        foreach (Vector3 coord in hottubCoordinates)
        {
            if (!npcManager.occupiedHottubCoordinates.Contains(coord))
            {
                npcManager.occupiedHottubCoordinates.Add(coord);
                hotTubPosition = coord;
                transform.position = coord;
                rb.velocity = Vector2.zero; // Stop any movement
                assignedToHottub = true;
                break;
            }
        }

        if (!assignedToHottub)
        {
            Debug.Log("No more hottub coordinates available");
            SetNewTargetLocation(Location.Pool);
        }
        else
        {
            float timeSpentInHottub = 0f;
            while (status == NPCStatus.Hottub)
            {
                timeSpentInHottub += Time.deltaTime;
                if (timeSpentInHottub >= eventData.preheatDuration)
                {
                    IEvent overheat = eventLoop.CreateEventNPC(EventType.OverHeating, this);
                    eventManager.TriggerEvent(overheat);
                    timeSpentInHottub = 0f; // Reset the timer after overheating
                }
                yield return null;
            }

        }

    }

    //Only triggered when exiting the box collider
    void OnTriggerExit2D(Collider2D collision)
    {
        Vector2 newVelocity = rb.velocity;
        switch (status)
        {
            case NPCStatus.Swimming:
                if (collision.gameObject.tag == "Pool")
                {
                    newVelocity *= -1;
                }
                break;
            case NPCStatus.Running:
                if (collision.gameObject.tag == "RunningArea")
                {
                    newVelocity *= -1;
                }
                break;
            //Allow npc to pass through if travelling
            case NPCStatus.Travelling:
                break;
        }
        rb.velocity = newVelocity;
    }

    //Updates targetLocation called from start
    private void SetSpawnTargetLocation()
    {
        //Set travelling status to signal main update loop
        status = NPCStatus.Travelling;
        // subtract 1 to exclude None
        //targetLocation = (Location)Random.Range(0, System.Enum.GetValues(typeof(Location)).Length - 1);

        float randomValue = Random.Range(0f, 1f);
        if (randomValue < 0.8f)
        {
            targetLocation = Location.Pool;
        }
        else
        {
            targetLocation = Location.Hottub;
        }

        switch (targetLocation)
        {
            case Location.Pool:
                pathQueue.Enqueue(new Vector3(-4, -8, 0));
                pathQueue.Enqueue(new Vector3(-4, -7, 0));
                break;
            case Location.Hottub:
                pathQueue.Enqueue(new Vector3(12, -7, 0));
                break;
                //Add more destination coordinates here
        }
    }

    public void SetNewTargetLocation(Location location)
    {
        pathQueue.Enqueue(npcManager.spawnLocation);
        status = NPCStatus.Travelling;
        targetLocation = location;

        switch (targetLocation)
        {
            case Location.Pool:
                pathQueue.Enqueue(new Vector3(-4, -8, 0));
                pathQueue.Enqueue(new Vector3(-4, -7, 0));
                break;
            case Location.Hottub:
                pathQueue.Enqueue(new Vector3(12, -7, 0));
                break;
            case Location.RunningArea:
                pathQueue.Enqueue(new Vector3(-16, -1, 0));
                break;
                //Add more destination coordinates here
        }
    }

    public void SetNewTargetLocationCoords(Vector2Int coords, Location location)
    {
        targetLocation = location;
        status = NPCStatus.Travelling;
        pathQueue.Enqueue(new Vector3(coords.x, coords.y, 0));
    }


    public Vector2Int GetLocation()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    public void Leave()
    {
        Debug.Log("NPC " + name + " left the pool. U suck!");
        //Place holder value. Will update once we have exit coords.
        //Move(new Vector2Int(0, 0));
    }

    public void DealDamage(int damage)
    {
        Debug.Log(name + " took " + damage + " damage");
        health -= damage;
        if (health < 0)
        {
            die();
        }
    }

    public bool GetIsEventOccuring()
    {
        return isEventOccuring;
    }

    public void SetIsEventOccuring(bool value)
    {
        isEventOccuring = value;
    }

    public void LowerSatisfaction(float amount)
    {
        Debug.Log(name + " lost " + amount + " satisfaction");
        satisfaction -= amount;
        if (satisfaction <= 0)
        {
            Leave();
        }
    }

    public void IncreaseSatisfaction(float amount) { satisfaction += amount; }

    public void SetStatus(NPCStatus newStatus)
    {
        status = newStatus;
    }

    public string GetName()
    {
        return name;
    }

    public int GetHealth()
    {
        return health;
    }

    public NPCStatus GetStatus()
    {
        return status;
    }
    public float GetSatisfaction()
    {
        return satisfaction;
    }

    public float GetMaxSatisfaction()
    {
        return startingSatisfaction;
    }

    //We destroy the game object in NPCManager.cs
    void die()
    {
        Debug.Log("NPC " + name + " died");
        SetStatus(NPCStatus.Dead);
    }

    public float CalculateSatisfactionPercentage()
    {
        return (float)satisfaction / (float)startingSatisfaction;
    }
    public void SetIcecreamPosition(Vector2Int position)
    {
        icecreamPosition = position;
    }
    public Vector2Int GetIcecreamPosition()
    {
        return icecreamPosition;
    }

}
