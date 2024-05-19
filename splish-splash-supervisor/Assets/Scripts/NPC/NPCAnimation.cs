using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    public List<Sprite> nSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    
    public List<Sprite> sIdleSprites;
    public List<Sprite> selectedSprites;
    private SpriteRenderer spriteRenderer;
    public float frameRate;
    private float changeX;
    public float changeCutoff;
    private float changeY;
    private Rigidbody2D rb;
    private NPC npc;

    private int facing = 1;// 1 for east, 2 for north, 3 for south

    private Vector3 lastPosition;
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        npc = GetComponent<NPC>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        selectedSprites = eSprites;
    }

    // Update is called once per frame
    void Update()
    {
        if (npc.GetStatus() == NPCStatus.Hottub)
        {
            spriteRenderer.flipX = false;

        }
        else
        {
            if (!spriteRenderer.flipX && changeX > 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (spriteRenderer.flipX && changeX < 0f)
            {
                spriteRenderer.flipX = false;
            }
        }


        if (Mathf.Abs(changeX) > changeCutoff && Mathf.Abs(changeY) > changeCutoff)
        {
            SetSprite();
        }
        else if (rb.velocity.magnitude > 0.1f)
        {
            SetSpriteRB();
        }
        else
        {
            selectedSprites = sIdleSprites;
        }

        if (transform.position != lastPosition)
        {
            changeX = transform.position.x - lastPosition.x;
            changeY = transform.position.y - lastPosition.y;
            direction = (transform.position - lastPosition).normalized;
        }

        lastPosition = transform.position;

        int frame = (int)(Time.time * frameRate % 5);


        spriteRenderer.sprite = selectedSprites[frame];
    }

    void SetSprite()
    {
        if (Mathf.Abs(changeY) > Mathf.Abs(changeX) && changeY > 0)
        {
            selectedSprites = nSprites;
            facing = 2;
        }
        else if (Mathf.Abs(changeY) > Mathf.Abs(changeX) && changeY < 0)
        {
            selectedSprites = sSprites;
            facing = 3;
        }
        else if (Mathf.Abs(changeY) < Mathf.Abs(changeX))
        {
            selectedSprites = eSprites;
            facing = 1;
        }
    }

    void SetSpriteRB()
    {
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            selectedSprites = eSprites;
            facing = 1;
        }
        else if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(rb.velocity.y) && rb.velocity.y > 0)
        {
            selectedSprites = nSprites;
            facing = 2;
        }
        else if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(rb.velocity.y) && rb.velocity.y < 0)
        {
            selectedSprites = sSprites;
            facing = 3;
        }
    }
}