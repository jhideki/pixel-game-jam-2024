using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DrowningMiniGame : MonoBehaviour
{
    public KeyCode[] sequenceKeys;
    public Sprite upArrow;
    public Sprite downArrow;
    public Sprite rightArrow;
    public Sprite leftArrow;
    private MiniGameTimer timer;
    public Sprite imageSprite;
    private Text miniGameTimer;
    //In seconds
    public float timeBuffer = 0.5f;
    private int currendKeyIndex = 0;
    private MiniGame miniGame;
    private ImageManager imageManager;
    private ImageManager arrowImageManager;
    private bool loading = false;
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
        while (miniGame.GetStatus() == MiniGameStatus.Playing)
        {
            miniGameTimer.text = (timeBuffer - timer.GetCurrentTime()).ToString();

            if (timer.GetCurrentTime() >= timeBuffer)
            {
                miniGame.SetStatus(MiniGameStatus.Lose);
                arrowImageManager.ToggleImage();
            }

            switch (sequenceKeys[currendKeyIndex])
            {
                case KeyCode.UpArrow:
                    arrowImageManager.SetImage(upArrow);
                    break;

                case KeyCode.DownArrow:
                    arrowImageManager.SetImage(downArrow);
                    break;

                case KeyCode.RightArrow:
                    arrowImageManager.SetImage(rightArrow);
                    break;

                case KeyCode.LeftArrow:
                    arrowImageManager.SetImage(leftArrow);
                    break;
            }

            if (Input.GetKeyDown(sequenceKeys[currendKeyIndex]))
            {
                currendKeyIndex += 1;
                if (currendKeyIndex >= sequenceKeys.Length)
                {
                    miniGame.SetStatus(MiniGameStatus.Win);
                    currendKeyIndex = 0;
                    arrowImageManager.ToggleImage();
                }

            }
            else if (Input.anyKeyDown)
            {
                currendKeyIndex = 0;
                arrowImageManager.ShakeImage();
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
