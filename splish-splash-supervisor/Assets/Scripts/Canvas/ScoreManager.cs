using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Change to TextMeshProUGUI
    private NPCManager npcManager;
    public int initialScore = 100;
    public float score;
    public string endingName = "ending";
    
    // Start is called before the first frame update
    void Start()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        }
        npcManager = GameObject.Find("EventManager").GetComponent<NPCManager>();
        score = initialScore; // Set the initial score
    }

    void Update()
    {
        score = (npcManager.GetSatisfaction() / npcManager.GetStartingSatisfaction()) * 100;
        scoreText.text = score.ToString("F2");
    }

    /*
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void DeleteScore(int points)
    {
        score -= points;
        UpdateScoreText();

        if(score <= 0)
        {
            SceneManager.LoadScene(endingName);
        }
    }
    

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
    */
    
}
