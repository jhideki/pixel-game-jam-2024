using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    //Pointer to current game
    private GameObject currentMiniGameObject;
    private MiniGame currentMiniGame;
    //Example of how to initialize a minigame based off an event
    public GameObject drowningMiniGamePrefab;
    private GameObject miniGameCanvas;
    void Start()
    {

        miniGameCanvas = GameObject.Find("Canvas/MiniGame");
        miniGameCanvas.SetActive(false);
    }

    public bool IsRunning()
    {
        return currentMiniGame != null;
    }
    //TODO: Add more minigames prefabs here
    public void StartMiniGame(IEvent e)
    {
        EventType type = e.Type;
        switch (type)
        {
            /*Copy this example for the other mini games. Each mini game prefab must have a script component that initilizes an instance of the MiniGame class and 
            contains a function called Initialize() that returns type MiniGame*/
            case EventType.Drowning:
                //Maybe change this to spawn the object as a child of the MiniGameController game object
                miniGameCanvas.SetActive(true);
                currentMiniGameObject = Instantiate(drowningMiniGamePrefab, new Vector3(e.location.x, e.location.y, 0), Quaternion.identity);
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
        if (currentMiniGame != null)
        {
            if (currentMiniGame.GetStatus() == MiniGameStatus.Win)
            {
                miniGameCanvas.SetActive(false);
                Destroy(currentMiniGameObject);
            }

            if (currentMiniGame.GetStatus() == MiniGameStatus.Lose)
            {
                miniGameCanvas.SetActive(false);
                Destroy(currentMiniGameObject);
                currentMiniGame.SetStatus(MiniGameStatus.Complete);
            }
        }
    }

}
