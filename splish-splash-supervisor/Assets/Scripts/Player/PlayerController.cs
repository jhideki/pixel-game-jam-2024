using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    float speedX, speedY;
    private EventManager eventManager;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
                Debug.Log("Colided with event");
                //TODO minigame
                //Create minigame object + class script with function StartMiniGame()
                eventManager.EndEvent(collision.gameObject);
            }
        }
    }
}
