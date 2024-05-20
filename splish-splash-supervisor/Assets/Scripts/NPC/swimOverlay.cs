using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swimOverlay : MonoBehaviour
{
    public List<Sprite> swimSprites;
    private SpriteRenderer spriteRenderer;
    public float frameRate;

    private NPC npc;

    private bool inWater;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        npc = GetComponentInParent<NPC>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        int frame = (int)(Time.time * frameRate % 5);
        spriteRenderer.sprite = swimSprites[frame];

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