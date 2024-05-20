using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrowningMiniGame : MonoBehaviour
{
    public float timeLimit = 4f;
    private MiniGameTimer timer;
    public Sprite imageSprite;
    //In seconds
    private MiniGame miniGame;
    private bool loading = false;

    public GameObject movingBlock;
    public GameObject target;
    // Start is called before the first frame update
    public MiniGame Initialize()
    {
        //Set image sprite

        //Set up class instances
        miniGame = new MiniGame();
        timer = GetComponent<MiniGameTimer>();

        //Load minigame image
        loading = true;
        StartCoroutine(RunGame());
        return miniGame;
    }


    IEnumerator RunGame()
    {
        timer.StartTimer();
        MovingBlock mb = movingBlock.GetComponent<MovingBlock>();
        Target t = target.GetComponent<Target>();
        mb.Init();

        while (miniGame.GetStatus() == MiniGameStatus.Playing)
        {
            if (timer.GetCurrentTime() > timeLimit)
            {
                miniGame.SetStatus(MiniGameStatus.Lose);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                mb.Toggle();
            }

            if (t.HasCollided && !mb.IsMoving())
            {
                yield return new WaitForSeconds(1f);
                miniGame.SetStatus(MiniGameStatus.Win);
                break;
            }

            if (!mb.IsMoving())
            {
                miniGame.SetStatus(MiniGameStatus.Lose);
                yield return new WaitForSeconds(1f);
                break;
            }
            yield return null;
        }
    }
}