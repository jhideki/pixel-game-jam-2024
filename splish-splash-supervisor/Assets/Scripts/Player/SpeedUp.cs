using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    private PlayerController playerController;
    public float speedUpAmount = 10f;
    public float duration = 5f;
    public float coolDown = 30f;

    private bool isAllowed = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update    
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();

         // Initially disable rendering and interaction
        spriteRenderer.enabled = false;
        isAllowed = false;

        StartCoroutine(SpeedUpCooldown());
    }

    void Update()
    {
        spriteRenderer.enabled = isAllowed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player collided with the speed-up object
        if (other.gameObject.CompareTag("Player") && isAllowed)
        {
            if (playerController != null)
            {
                playerController.speed = playerController.speed + speedUpAmount;
                Debug.Log("Speed increased to " + playerController.speed);

                // Immediately disable interaction and rendering
                isAllowed = false;
                spriteRenderer.enabled = false;
            }

        }
    }

    private IEnumerator SpeedUpCooldown()
    {
        while (true)
        {
            // Allow interaction for a specified duration
            isAllowed = true;
            yield return new WaitForSeconds(duration);

            // Disable interaction for a specified cooldown period
            isAllowed = false;
            yield return new WaitForSeconds(coolDown);
        }
    }
}
