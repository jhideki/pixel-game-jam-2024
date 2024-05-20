using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private float time = 0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 1.0f)
        { Destroy(gameObject); }
    }
}
