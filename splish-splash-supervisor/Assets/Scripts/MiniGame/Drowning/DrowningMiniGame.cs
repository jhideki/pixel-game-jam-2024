using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DrowningMiniGame : MonoBehaviour
{
    private MiniGameTimer timer;
    public Sprite imageSprite;
    private Text miniGameTimer;
    //In seconds
    private MiniGame miniGame;
    private ImageManager imageManager;
    private ImageManager arrowImageManager;
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
        arrowImageManager = GameObject.Find("Canvas/MiniGame/MiniGameKeyImage").GetComponent<ImageManager>();
        miniGameTimer = GameObject.Find("Canvas/MiniGame/MiniGameTimerText").GetComponent<Text>();

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

    //TODO: visual feedback
    IEnumerator RunGame()
    {
        arrowImageManager.ToggleImage();
        timer.StartTimer();
        MovingBlock mb = movingBlock.GetComponent<MovingBlock>();

        while (miniGame.GetStatus() == MiniGameStatus.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                mb.Toggle();
            }
            BoxCollider2D mbCollider = movingBlock.GetComponent<BoxCollider2D>();
            BoxCollider2D targetCollider = target.GetComponent<BoxCollider2D>();

            Gizmos.DrawWireCube(mbCollider.bounds.center, mbCollider.bounds.size);
            Gizmos.DrawWireCube(targetCollider.bounds.center, targetCollider.bounds.size);

            if (movingBlock.GetComponent<BoxCollider2D>().bounds.Intersects(target.GetComponent<BoxCollider2D>().bounds) && !mb.IsMoving())
            {
                Debug.Log("----- u won");
            }
            else if (!mb.IsMoving())
            {
                yield return new WaitForSeconds(2f);
                mb.Toggle();
            }
            yield return null;
        }

        //Fade out image
        loading = true;
        StartCoroutine(imageManager.FadeImage(false, OnImageLoaded));
        while (loading)
        {
            yield return null;
        }
    }
}