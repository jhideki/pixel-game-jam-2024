using TMPro;
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
    private bool isAtIcecreamStand;
    private NPCLine icecreamLine;

    void Start()
    {
        isCollidingEvent = false;
        miniGameController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        icecreamLine = GameObject.Find("IcecreamStand").GetComponent<NPCLine>();
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

        if (isAtIcecreamStand && Input.GetKeyDown(KeyCode.Space))
        {
            icecreamLine.DequeueLine();
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
        else if (collision.gameObject.tag == "IcecreamStand")
        {
            isAtIcecreamStand = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Event")
        {
            isCollidingEvent = false;
            currentEvent = collision.gameObject;
        }
        else if (collision.gameObject.tag == "IcecreamStand")
        {
            isAtIcecreamStand = false;
        }
    }
    private void EndEvent()
    {
        EventObject eventObject = currentEvent.GetComponent<EventObject>();
        IEvent e = eventObject.GetEvent();
        if (e.isActive)
        {
            if (e.Type == EventType.Drowning && !miniGameController.IsRunning())
            {
                // Run minigame
                miniGameController.StartMiniGame(currentEvent);
            }
            else if (e.Type == EventType.OverHeating)
            {
                eventManager.EndEvent(currentEvent);
            }
            else if (e.Type != EventType.Drowning)
            {
                eventManager.EndEvent(currentEvent);
            }
        }
    }
}
