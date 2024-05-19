using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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

    private Rigidbody2D rb;
    private Queue<Vector3> pathQueue = new Queue<Vector3>();

    private static List<Vector3> hottubCoordinates = new List<Vector3>
    {
        new Vector3(14, 2, 0),
        new Vector3(14, 3, 0),
        new Vector3(13, 3, 0),
        new Vector3(12, 3, 0),
        new Vector3(12, 2, 0),
        new Vector3(12, 1, 0),
        new Vector3(13, 1, 0),
        new Vector3(14, 1, 0),
        // Add more coordinates as needed
    };

    private static HashSet<Vector3> occupiedHottubCoordinates = new HashSet<Vector3>();

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        eventLoop = GameObject.Find("EventManager").GetComponent<EventLoop>();


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
                if (!isStatusRoutineRunning)
                {
                    StartCoroutine(MoveTowardsTarget());
                }
                break;
            case NPCStatus.Swimming:
                if (!isStatusRoutineRunning)
                {
                    StartCoroutine(Swim());
                }
                break;
            case NPCStatus.Hottub:
                if (!isStatusRoutineRunning)
                {
                    StartCoroutine(Hottub());
                }
                break;
            case NPCStatus.EventOccuring:
                StartCoroutine(EventOccuring());
                break;
        }
    }
    IEnumerator EventOccuring()
    {
        isStatusRoutineRunning = true;
        rb.velocity = Vector2.zero;
        while (status == NPCStatus.EventOccuring)
        {
            yield return null;
        }
        isStatusRoutineRunning = false;
    }

    IEnumerator MoveTowardsTarget()
    {
        isStatusRoutineRunning = true;
        //Check if queue is empty and if there is a target location, if not set new target and move towards target
        while (pathQueue.Count > 0 && targetLocation != Location.None)
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
        }

        isStatusRoutineRunning = false;
        //Set target location to none
        targetLocation = Location.None;
    }


    IEnumerator Swim()
    {
        while (status == NPCStatus.Swimming)
        {
            //Set flag so we don't spawn multiple coroutines
            isStatusRoutineRunning = true;
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
        isStatusRoutineRunning = true;
        bool assignedToHottub = false;

        foreach (var coord in hottubCoordinates)
        {
            if (!occupiedHottubCoordinates.Contains(coord))
            {
                occupiedHottubCoordinates.Add(coord);
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
            yield break;
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

            // Free up the coordinate when NPC leaves the hot tub
            occupiedHottubCoordinates.Remove(transform.position);
        }

        isStatusRoutineRunning = false;
    }

    //Only triggered when exiting the box collider
    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision status : " + status);
        Vector2 newVelocity = rb.velocity;
        switch (status)
        {
            case NPCStatus.Hottub:
                if (collision.gameObject.tag == "Hottub")
                {
                    newVelocity *= -1;
                }
                break;
            case NPCStatus.Swimming:
                if (collision.gameObject.tag == "Pool")
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
                pathQueue.Enqueue(new Vector3(5, -8, 0));
                pathQueue.Enqueue(new Vector3(5, -3, 0));
                break;
            case Location.Hottub:
                pathQueue.Enqueue(new Vector3(13, 2, 0));
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
                pathQueue.Enqueue(new Vector3(5, -8, 0));
                pathQueue.Enqueue(new Vector3(5, -3, 0));
                break;
            case Location.Hottub:
                pathQueue.Enqueue(new Vector3(13, 2, 0));
                break;
                //Add more destination coordinates here
        }
    }

    public void SetNewTargetLocationCoords(Vector2Int coords)
    {
        pathQueue.Enqueue(npcManager.spawnLocation);
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

}
