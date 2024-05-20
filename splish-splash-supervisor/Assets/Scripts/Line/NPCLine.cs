using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPCLine : MonoBehaviour
{
    private LinkedList<GameObject> deque = new LinkedList<GameObject>();
    private Vector2Int nextLocation;
    private Vector2Int startLocation;
    private int length;
    public int capacity = 5;
    public Direction direction;
    void Start()
    {
        startLocation = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        if (direction == Direction.West)
        {
            nextLocation = new Vector2Int(startLocation.x + 1, startLocation.y);
        }
        else
        {
            nextLocation = new Vector2Int(startLocation.x - 1, startLocation.y);
        }
    }

    public Vector2Int GetNextLocation()
    {
        return nextLocation;
    }

    public bool IsFull()
    {
        return capacity >= length;
    }

    public void DequeueLine()
    {
        Debug.Log("---dequeing line");
        GameObject npc = deque.First.Value;
        deque.RemoveFirst();
        npc.GetComponent<NPC>().SetNewTargetLocation(Location.Pool);
        foreach (var npcObject in deque)
        {
            NPC n = npcObject.GetComponent<NPC>();

            if (direction == Direction.West)
            {
                n.SetNewTargetLocationCoords(new Vector2Int(n.GetIcecreamPosition().x - 1, n.GetIcecreamPosition().y), Location.IcecreamStand);
            }
            else
            {
                n.SetNewTargetLocationCoords(new Vector2Int(n.GetIcecreamPosition().x + 1, n.GetIcecreamPosition().y), Location.IcecreamStand);
            }
        }
        if (direction == Direction.West)
        {
            nextLocation = new Vector2Int(nextLocation.x - 1, nextLocation.y);
        }
        else
        {
            nextLocation = new Vector2Int(nextLocation.x + 1, nextLocation.y);
        }
        length--;
    }

    public void EnqueueLine(GameObject npc)
    {
        foreach (var val in deque)
        {
            Debug.Log("---- val: " + val.name);
        }
        length++;
        if (direction == Direction.West)
        {
            nextLocation = new Vector2Int(nextLocation.x + 1, nextLocation.y);
        }
        else
        {
            nextLocation = new Vector2Int(nextLocation.x - 1, nextLocation.y);
        }
        deque.AddLast(npc);
    }

}
