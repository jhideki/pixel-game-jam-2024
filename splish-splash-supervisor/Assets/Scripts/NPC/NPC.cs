using UnityEngine;
using System.Collections.Generic;

public class NPC : MonoBehaviour
{
    public NPCParameters parameters;
    private int health;
    private float satisfaction;
    private float startingSatisfaction;
    private string name;
    private NPCStatus status;
    private NPCManager npcManager;
    private bool isEventOccuring;

    private Queue<Vector3> pathQueue = new Queue<Vector3>();
    private Vector3 targetPosition;
    private bool isMoving = true;
    public float moveSpeed = 5f; // Adjust the speed as needed

    void Start()
    {
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        name = parameters.names[Random.Range(0, parameters.names.Length)];
        health = Random.Range(parameters.minHealth, parameters.maxHealth);
        satisfaction = Random.Range(parameters.minSatisfaction, parameters.maxSatisfaction);
        startingSatisfaction = satisfaction;
        Debug.Log("Created " + name + " with health " + health + " and satisfaction " + satisfaction);
        GetPath();

        // Start moving along the path if there are waypoints
        if (pathQueue.Count > 0)
        {
            targetPosition = pathQueue.Dequeue();
            isMoving = true;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
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

    // Move the NPC towards the target position
    void MoveTowardsTarget()
    {
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else if (pathQueue.Count > 0)
        {
            targetPosition = pathQueue.Dequeue();
        }
        else
        {
            isMoving = false; // Stop moving when the path is complete
        }
    }

    //TODO: Move
    public void SetMove(int x, int y)
    {
        pathQueue.Enqueue(new Vector3(x, y, 0));
    }
    
    public void GetPath()
    {
        int z = Random.Range(1, 11);
        if (z <= 5)
        {
            PathOne();
        }
        else
        {
            Pathtwo();
        }
    }

    public void PathOne()
    {
        SetMove(12, 1);
    }

    public void Pathtwo()
    {
        SetMove(0, -8);
        SetMove(0, -5);
    }

    public Vector2Int GetLocation()
    {
        return new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    //TODO: Leave (will call Move() to leave pool. Once move is done destroy game object)
    public void Leave()
    {
        Debug.Log("NPC " + name + " left the pool. U suck!");
        //Place holder value. Will update once we have exit coords.
        //Move(new Vector2Int(0, 0));
    }
}
