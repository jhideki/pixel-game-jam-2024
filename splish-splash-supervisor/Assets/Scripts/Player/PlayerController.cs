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
    private bool isCollidingEvent;
    private GameObject currentEvent;
    private NPCManager npcManager;

    void Start()
    {
        isCollidingEvent = false;
        miniGameController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GameObject.Find("Oval").GetComponent<SpriteRenderer>();
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
        if (isCollidingEvent && Input.GetKeyDown(KeyCode.Space))
        {
            EndEvent();
        }

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
            if (isCollidingEvent)
            {
                EndEvent();
            }
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }

        //RotatePlayer();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Event")
        {
            isCollidingEvent = true;
            currentEvent = collision.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Event")
        {
            isCollidingEvent = false;
            currentEvent = collision.gameObject;
        }
    }
    private void EndEvent()
    {
        EventObject eventObject = currentEvent.GetComponent<EventObject>();
        IEvent e = eventObject.GetEvent();
        if (e.isActive)
        {
            if (e.Type == EventType.Drowning)
            {
                // Run minigame
                miniGameController.StartMiniGame(e);
                eventManager.EndEvent(currentEvent);
            }
            else if (e.Type == EventType.OverHeating)
            {
                eventManager.EndEvent(currentEvent);
            }
            else
            {
                eventManager.EndEvent(currentEvent);
            }
        }
    }
}
