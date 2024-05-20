using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    private IEvent e;
    private BoxCollider2D boxCollider2D;
    private Transform transfrom;

    public void SetEvent(IEvent e)
    {

        boxCollider2D = GetComponent<BoxCollider2D>();
        this.e = e;
        boxCollider2D.size = new Vector2(e.Size, e.Size);
        transform.position = new Vector3(e.location.x, e.location.y, 0);
    }

    void Update()
    {
        if (e.Type == EventType.Running)
        {
            transform.position = e.nPC.transform.position;
        }
    }

    public IEvent GetEvent()
    {
        return e;
    }

}
