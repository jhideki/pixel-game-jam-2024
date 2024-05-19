using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int initialScore = 0;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        }

        score = initialScore;
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void DeleteScore(int points)
    {
        score -= points;
        UpdateScoreText();
    }
    

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
