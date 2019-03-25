using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;

    public Text scoreText;
    public int score;
    public Image scoreBar;

    private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
    }

    public void IncreaseScore(int amountToIncrease)
    {
        // add the amountToIncrease to the score
        score += amountToIncrease;

        // check to see if the game data isn't null
        if (gameData != null)
        {
            int hiScore = gameData.saveData.highScores[board.level];
            if (score > hiScore)
            {
                gameData.saveData.highScores[board.level] = score;
            }
            gameData.Save();
        }

        // Fill the score bar
        if (board != null && scoreBar != null)
        {
            int length = board.scoreGoals.Length;
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }
    }
}
