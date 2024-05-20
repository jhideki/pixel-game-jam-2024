using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public List<Sprite> nSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    public List<Sprite> sIdleSprites;
    public List<Sprite> eIdleSprites;
    public List<Sprite> nIdleSprites;

    private swimOverlay overlay;
    private WaterOverlay waterOverlay;
    private List<Sprite> selectedSprites;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    public float frameRate;
    private int facing = 1;// 1 for east, 2 for north, 3 for south

    //Underwater variables
    private bool playingAnimation;
    public float animationBuffer = 2.0f;
    private float appearStartTime;
    public bool isTimingAppear;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedSprites = eSprites;
        anim = GetComponent<Animator>();
        anim.enabled = false;
        playingAnimation = false;
        isTimingAppear = false;
        animator = GetComponent<Animator>();
        animator.enabled = false;
        overlay = transform.Find("SwimOverlay").GetComponent<swimOverlay>();
        waterOverlay = transform.Find("WaterOverlay").GetComponent<WaterOverlay>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - appearStartTime) > animationBuffer && playingAnimation)
        {
            anim.enabled = false;
            playingAnimation = false;
            isTimingAppear = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Whistle());
        }


        if (!spriteRenderer.flipX && rb.velocity.x > 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && rb.velocity.x < 0f)
        {
            spriteRenderer.flipX = false;
        }

        // setsprite if moving else set idle sprite
        if (rb.velocity.magnitude > 0.1f)
        {

            SetSprite();
        }
        else
        {
            selectedSprites = sIdleSprites;
        }
        int frame = (int)(Time.time * frameRate % 6);

        spriteRenderer.sprite = selectedSprites[frame];
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Pool" || collider.gameObject.tag == "Hottub")
        {
            overlay.SetInWater(true);
            waterOverlay.SetInWater(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Pool" || collider.gameObject.tag == "Hottub")
        {
            overlay.SetInWater(false);
            waterOverlay.SetInWater(false);
        }
    }

    IEnumerator Whistle()
    {
        animator.enabled = true;
        yield return new WaitForSeconds(1.2f);
        animator.enabled = false;
    }

    void SetSprite()
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
