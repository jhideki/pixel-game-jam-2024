using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    float speedX, speedY;
    private EventManager eventManager;
    public MiniGameController miniGameController;

    Rigidbody2D rb;

    void Start()
    {
        miniGameController = GameObject.Find("MiniGameController").GetComponent<MiniGameController>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal") * speed;
        speedY = Input.GetAxisRaw("Vertical") * speed;
        rb.velocity = new Vector2(speedX, speedY);
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
