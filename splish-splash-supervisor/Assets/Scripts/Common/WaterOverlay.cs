using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterOverlay : MonoBehaviour
{

    private bool inWater;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (inWater)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }

    }
    public void SetInWater(bool val)
    {
        inWater = val;
    }
}