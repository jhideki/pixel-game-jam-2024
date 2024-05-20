using UnityEngine;

public class MiniGameTimer : MonoBehaviour
{
    private bool isRunning;
    private float currentTime;//time in seconds


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
        }
    }
    public float GetCurrentTime()
    {
        return currentTime;
    }
    public void ResetTimer()
    {
        currentTime = 0f;
    }
}
