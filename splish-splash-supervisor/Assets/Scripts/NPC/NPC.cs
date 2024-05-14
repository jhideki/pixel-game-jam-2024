using UnityEngine;

public class NPC : MonoBehaviour
{
    public NPCParameters parameters;
    private int health;
    private float satisfaction;
    private float startingSatisfaction;
    private string name;
    private NPCStatus status;
    private NPCManager npcManager;
    public BoxCollider2D collider2D;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        name = parameters.names[Random.Range(0, parameters.names.Length)];
        health = Random.Range(parameters.minHealth, parameters.maxHealth);
        satisfaction = Random.Range(parameters.minSatisfaction, parameters.maxSatisfaction);
        startingSatisfaction = satisfaction;
        Debug.Log("Created " + name + " with health " + health + " and satisfaction " + satisfaction);
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

    //We destroy the game object in NPCManager.cs
    void die()
    {
        Debug.Log("NPC " + name + " died");
        SetStatus(NPCStatus.Dead);
    }

    public float calculateSatisfactionPercentage()
    {
        return (float)satisfaction / (float)startingSatisfaction;
    }

    //TODO: Move
    public void Move(Vector2Int destination)
    {

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
        Move(new Vector2Int(0, 0));
    }
}
