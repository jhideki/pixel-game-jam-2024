using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLine : MonoBehaviour
{
    private LinkedList<GameObject> deque = new LinkedList<GameObject>();
    private Vector2Int nextLocation;
    private Vector2Int startLocation;
    private int length;
    public NPCLine(Vector2Int location)
    {
        startLocation = location;
        nextLocation = new Vector2Int(startLocation.x, startLocation.y + 1);
    }

    public Vector2Int GetNextLocation()
    {
        return nextLocation;
    }

    public GameObject DequeueLine()
    {
        GameObject npc = deque.First.Value;
        deque.RemoveLast();
        nextLocation = new Vector2Int(nextLocation.x, nextLocation.y - 1);
        return npc;
    }

    public void EnqueueLine(GameObject npc)
    {
        length++;
        nextLocation = new Vector2Int(nextLocation.x, nextLocation.y + 1);
        deque.AddLast(npc);
    }

}
