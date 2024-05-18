using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 velocity;
    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = velocity;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void Toggle()
    {
        if (isMoving)
        {
            isMoving = false;
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            isMoving = true;
            rb.velocity = velocity;

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "Bar")
        {
            Debug.Log("------gay");
            velocity = new Vector2(0, rb.velocity.y * -1);
            rb.velocity = velocity;
        }

    }

}
