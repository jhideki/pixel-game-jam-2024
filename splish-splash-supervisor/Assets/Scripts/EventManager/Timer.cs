using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool isRunning;
    private float currentTime;//time in seconds
    private int dayCount;
    public float dayDuration;


    // Start is called before the first frame update
    public void StartTimer()
    {
        isRunning = true;
        currentTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= dayDuration)
            {
                currentTime = 0f;
                dayCount += 1;
                Debug.Log("Day " + dayCount);
            }
        }
    }
    public String GetCurrentTime()
    {
        int minutes = (int)currentTime / 60;
        int seconds = (int)currentTime % 60;
        return minutes + ":" + seconds;
    }
    public float GetExactTime()
    {
        return currentTime;
    }
    public float GetCurrentDay()
    {
        return dayCount;
    }
    public void ResetTimer()
    {
        currentTime = 0f;
    }
}
