using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    MiniGameTimer timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = new MiniGameTimer();
        timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer.GetCurrentTime() > 1.5f)
        { Destroy(gameObject); }
    }
}
