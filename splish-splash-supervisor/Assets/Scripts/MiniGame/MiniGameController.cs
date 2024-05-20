using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    //Pointer to current game
    private GameObject currentMiniGameObject;
    private MiniGame currentMiniGame;
    //Example of how to initialize a minigame based off an event
    public GameObject drowningMiniGamePrefab;
    private GameObject newEvent;
    private IEvent curEvent;
    private EventManager eventManager;
    private bool isEventRunning = false;

    void Start()
    {
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
    }
    public bool IsRunning()
    {
        return isEventRunning;
    }
    //TODO: Add more minigames prefabs here
    public void StartMiniGame(GameObject eventObject)
    {
        isEventRunning = true;
        curEvent = eventObject.GetComponent<EventObject>().GetEvent();
        EventType type = curEvent.Type;
        newEvent = eventObject;
        switch (type)
        {
            /*Copy this example for the other mini games. Each mini game prefab must have a script component that initilizes an instance of the MiniGame class and 
            contains a function called Initialize() that returns type MiniGame*/
            case EventType.Drowning:
                //Maybe change this to spawn the object as a child of the MiniGameController game object
                currentMiniGameObject = Instantiate(drowningMiniGamePrefab, new Vector3(curEvent.location.x, curEvent.location.y, 0), Quaternion.identity);
                currentMiniGame = currentMiniGameObject.GetComponent<DrowningMiniGame>().Initialize();
                break;
            case EventType.Shitting:
                //TODO: Initialize Shitting minigame prefab and script component
                //E.g., 
                /*currentMiniGameObject = Instantiate(shittingMiniGamePrefab, new Vector3(e.location.x, e.location.y, 0), Quaternion.identity);
                currentMiniGame = currentMiniGameObject.GetComponent<ShittingMiniGame>().Initialize();
                */
                break;
            case EventType.Pissing:
                //TODO: Initialize Pissing minigame
                break;

            //Event does not have a minigame
            default:
                break;
                //TODO: Add more minigames here
        }
        //TODO: Initialize minigame based off event type. Event types are listed in Event.cs
    }

    void Update()
    {
        if (isEventRunning)
        {
            if (currentMiniGame.GetStatus() == MiniGameStatus.Win)
            {
                if (newEvent == null)
                {
                    Debug.Log("---new event is null");
                }
                eventManager.EndEventIEvent(curEvent);
                isEventRunning = false;
                currentMiniGame.SetStatus(MiniGameStatus.Complete);
                Destroy(currentMiniGameObject);
            }
            else if (currentMiniGame.GetStatus() == MiniGameStatus.Lose)
            {
                Debug.Log("---lost inside controller");
                eventManager.EndEventLose(curEvent);
                isEventRunning = false;
                currentMiniGame.SetStatus(MiniGameStatus.Complete);
                Destroy(currentMiniGameObject);
            }
        }
    }

}
