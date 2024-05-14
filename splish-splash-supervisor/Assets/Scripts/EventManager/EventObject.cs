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
        this.e = e;
        boxCollider2D.size = new Vector2(e.Size, e.Size);
        transform.position = new Vector3(e.location.x, e.location.y, 0);
    }

    public IEvent GetEvent()
    {
        return e;
    }
    // Start is called before the first frame update
    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

}
