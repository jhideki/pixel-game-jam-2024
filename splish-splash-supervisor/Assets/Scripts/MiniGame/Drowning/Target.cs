using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    float maxHeight = 2f;
    float minHeight = -1.69f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(minHeight, maxHeight), 0f);
    }

    // Update is called once per frame
}
