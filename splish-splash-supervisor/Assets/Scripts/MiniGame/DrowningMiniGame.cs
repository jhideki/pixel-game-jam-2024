using System.Collections;
using UnityEngine;

public class DrowningMiniGame : MonoBehaviour
{
    public KeyCode[] sequenceKeys;
    private MiniGameTimer timer;
    //In seconds
    public float timeBuffer = 0.5f;
    private int currendKeyIndex = 0;
    private MiniGame miniGame;
    // Start is called before the first frame update
    public MiniGame Initialize()
    {
        miniGame = new MiniGame();
        timer = GetComponent<MiniGameTimer>();
        timer.StartTimer();
        StartCoroutine(RunGame());
        return miniGame;
    }

    //TODO: visual feedback
    IEnumerator RunGame()
    {
        while (miniGame.GetStatus() == MiniGameStatus.Playing)
        {
            if (timer.GetCurrentTime() >= timeBuffer)
            {
                Debug.Log("You are failed!!!");
                miniGame.SetStatus(MiniGameStatus.Lose);
            }
            if (Input.GetKeyDown(sequenceKeys[currendKeyIndex]) && timer.GetCurrentTime() < timeBuffer)
            {
                Debug.Log("user hit key " + sequenceKeys[currendKeyIndex]);
                currendKeyIndex += 1;
                timer.ResetTimer();
                if (currendKeyIndex >= sequenceKeys.Length)
                {
                    Debug.Log("You passed the minigame!!!");
                    miniGame.SetStatus(MiniGameStatus.Win);
                    currendKeyIndex = 0;
                }

            }
            else if (Input.anyKeyDown)
            {
                Debug.Log("You hit the wrong key you retard");
                currendKeyIndex = 0;
            }
            yield return null;
        }
    }
}
