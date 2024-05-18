using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    float maxHeight = 2f;
    float minHeight = -1.69f;
    public bool HasCollided;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(minHeight, maxHeight), 0f);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MovingBlock")
        {
            HasCollided = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.name == "MovingBlock")
        {
            HasCollided = false;
        }
    }

    // Update is called once per frame
}
