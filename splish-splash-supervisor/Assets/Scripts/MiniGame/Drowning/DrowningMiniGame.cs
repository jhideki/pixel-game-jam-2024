using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DrowningMiniGame : MonoBehaviour
{
    public float timeLimit = 4f;
    private MiniGameTimer timer;
    public Sprite imageSprite;
    private Text miniGameTimer;
    private Text miniGameText;
    //In seconds
    private MiniGame miniGame;
    private ImageManager imageManager;
    private bool loading = false;

    public GameObject movingBlock;
    public GameObject target;

    void Start()
    {
        miniGame = Initialize();
    }
    // Start is called before the first frame update
    public MiniGame Initialize()
    {
        imageManager = GameObject.Find("Canvas/MiniGame/MiniGameBGImage").GetComponent<ImageManager>();
        miniGameTimer = GameObject.Find("Canvas/MiniGame/MiniGameTimerText").GetComponent<Text>();
        miniGameText = GameObject.Find("Canvas/MiniGame/MiniGameText").GetComponent<Text>();

        //Set image sprite
        imageManager.SetImage(imageSprite);

        //Set up class instances
        miniGame = new MiniGame();
        timer = GetComponent<MiniGameTimer>();
        miniGameTimer.text = "0.0";

        //Load minigame image
        loading = true;
        StartCoroutine(LoadImage());
        return miniGame;
    }

    private IEnumerator LoadImage()
    {
        StartCoroutine(imageManager.FadeImage(true, OnImageLoaded));
        while (loading)
        {
            yield return null;
        }
        timer.StartTimer();
        yield return StartCoroutine(RunGame());
    }

    void OnImageLoaded()
    {
        loading = false;
    }

    IEnumerator RunGame()
    {
        miniGameText.text = "";
        timer.StartTimer();
        MovingBlock mb = movingBlock.GetComponent<MovingBlock>();
        Target t = target.GetComponent<Target>();

        while (miniGame.GetStatus() == MiniGameStatus.Playing)
        {

            //update timer UI
            miniGameTimer.text = (timeLimit - timer.GetCurrentTime()).ToString();

            if (timer.GetCurrentTime() > timeLimit)
            {
                miniGame.SetStatus(MiniGameStatus.Lose);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                mb.Toggle();
            }

            if (t.HasCollided && !mb.IsMoving())
            {
                miniGameText.text = "YOU WIN!!!!!";
                yield return new WaitForSeconds(2f);
                miniGame.SetStatus(MiniGameStatus.Win);
            }
            else if (!mb.IsMoving())
            {
                miniGameText.text = "YOU LOSE!!!!! GET CLAPPED PUSSY";
                yield return new WaitForSeconds(2f);
                miniGame.SetStatus(MiniGameStatus.Lose);
            }
            yield return null;
        }

        //Cleanup
        loading = true;
        StartCoroutine(imageManager.FadeImage(false, OnImageLoaded));
        miniGameTimer.enabled = false;
        miniGameText.enabled = false;
        while (loading)
        {
            yield return null;
        }
    }
}