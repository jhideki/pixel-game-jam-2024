using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sped: MonoBehaviour
{
    public TextMeshProUGUI speed; // Change to TextMeshProUGUI
    //private float CurrentSpeed;
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        if (speed == null)
        {
            speed = GameObject.Find("SNum").GetComponent<TextMeshProUGUI>();
        }

        //CurrentSpeed = playerController.speed; // Set the initial score
    }

    void Update()
    {
        float CurrentSpeed = playerController.speed;
        speed.text = CurrentSpeed.ToString("F2");
    }
    
}
