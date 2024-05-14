using UnityEngine;

public class NPC : MonoBehaviour
{
    private Transform transform;
    public NPCParameters parameters;
    private int health;
    private float satisfaction;
    private float startingSatisfaction;
    private string name;

    void Start()
    {
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

    public string GetName()
    {
        return name;
    }

    public int GetHealth()
    {
        return health;

    }
    public float GetSatisfaction()
    {
        return satisfaction;
    }

    void die()
    {
        Debug.Log("NPC " + name + " died");
        Destroy(gameObject);
    }

    public float calculateSatisfactionPercentage()
    {
        return (float)satisfaction / (float)startingSatisfaction;
    }

    //TODO: Move
    public void Move(Vector2Int destination)
    {

    }

    //TODO: Leave (will call Move() to leave pool. Once move is done destroy game object)
    public void Leave()
    {
        Debug.Log("NPC " + name + " left the pool. U suck!");
        //Place holder value. Will update once we have exit coords.
        Move(new Vector2Int(0, 0));
    }
}
