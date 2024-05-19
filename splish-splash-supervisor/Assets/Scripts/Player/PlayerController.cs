using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    // float speedX, speedY;
    private EventManager eventManager;
    public MiniGameController miniGameController;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;

    private Vector2 movement = Vector2.zero;

    void Start()
    {
        miniGameController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GameObject.Find("Oval").GetComponent<SpriteRenderer>();
        if (capsuleCollider == null)
        {
            Debug.LogError("No CircleCollider2D found on the player object");
        }
        else
        {
            capsuleCollider.enabled = false; // Make sure the collider is initially disabled
        }

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the player object or its children");
        }
        else
        {
            spriteRenderer.enabled = false; // Initially disable the sprite
        }
    }

    void Update()
    {
        /*
        speedX = Input.GetAxisRaw("Horizontal") * speed;
        speedY = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(speedX, speedY);
        */
        // Capture input and lock movement to one direction

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            movement = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            movement = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            movement = Vector2.left;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            movement = Vector2.right;
        }
        else
        {
            movement = Vector2.zero;
        }

        rb.velocity = movement * speed;

        // Enable the collider when the player is whistling
        if (Input.GetKey(KeyCode.Space))
        {
            capsuleCollider.enabled = true;
            spriteRenderer.enabled = true;
        }
        else
        {
            capsuleCollider.enabled = false;
            spriteRenderer.enabled = false;
        }

        //RotatePlayer();
    }

    void RotatePlayer()
    {
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    void OnCapsolCollisionsEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Event")
        {
            EventObject eventObject = collision.gameObject.GetComponent<EventObject>();
            IEvent e = eventObject.GetEvent();
            if (e.isActive)
            {
                if (e.Type == EventType.Drowning)
                {
                    // Run minigame
                    miniGameController.StartMiniGame(e);
                    eventManager.EndEvent(collision.gameObject);
                }
                else if (e.Type == EventType.OverHeating)
                {
                    eventManager.EndEvent(collision.gameObject);
                }
            }
        }
    }
}
