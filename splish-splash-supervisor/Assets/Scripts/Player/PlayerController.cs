using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    float speedX, speedY;
    private EventManager eventManager;
    public MiniGameController miniGameController;

    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    void Start()
    {
        miniGameController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null)
        {
            Debug.LogError("No CircleCollider2D found on the player object");
        }
        else
        {
            circleCollider.enabled = false; // Make sure the collider is initially disabled
        }
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * speed;
        speedY = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(speedX, speedY);

        // Enable the collider when the player is whistling
        if (Input.GetKey(KeyCode.Space))
        {
            circleCollider.enabled = true;
        }
        else
        {
            circleCollider.enabled = false;
        }
    }

    void OnCollisionsEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Event")
        {
            EventObject eventObject = collision.gameObject.GetComponent<EventObject>();
            IEvent e = eventObject.GetEvent();
            if (e.isActive)
            {
                // Run minigame
                miniGameController.StartMiniGame(e);
                eventManager.EndEvent(collision.gameObject);
            }
        }
    }
}
