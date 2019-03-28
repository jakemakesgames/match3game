using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum GameType
{
    // An enum to decide whether the game type is moves or time
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirements
{
    public GameType gameType;
    public int counterValue; // A value to determine what the time should be set to OR how many moves the player has

}

public class EndGameManager : MonoBehaviour
{
    
    public EndGameRequirements requirements; // A reference to the EndGameRequirements Class
    public GameObject movesLabel; // A reference to the Moves Label UI element
    public GameObject timeLabel; // A reference to the Time Label UI element
    public TMP_Text counterText;
    //public Text counterText; // The moves/ time counter text
    public int currentCounterValue; // The current counter value
    private float timerSeconds; // The countdown timer (for time label)

    [Header("Win/ Lose Panels")]
    public GameObject youWinPanel;
    public GameObject tryAgainPanel;

    private Board board; // Make a ref to board

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        // Call the SetGameType function
        SetGameType();
        // Call the SetUpGame function
        SetUpGame();
    }

    void SetGameType()
    {
        if (board.world != null)
        {
            if (board.level < board.world.levels.Length)
            {
                if (board.world.levels[board.level] != null)
                {
                    // set the end game requirements
                    requirements = board.world.levels[board.level].endGameRequirements;
                }
            }
            
        }
    }

    // Check the gameType and set the UI accordingly
    void SetUpGame()
    {
        // Set the currentCounterValue equal to the counterValue stored in the requirements class
        currentCounterValue = requirements.counterValue;

        // If the game type IS Moves
        if (requirements.gameType == GameType.Moves)
        {
            // Moves label is active, time label is inactive
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);

        }
        else
        {
            timerSeconds = 1;
            // Else, the moves label is inactive, time label is active
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        // Set the counterText equal to the currentCounterValue
        counterText.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue()
    {
        // If the game is NOT in the pause state
        if (board.currentState != GameState.pause)
        {
            // Decrease the value, update the text
            currentCounterValue--;
            counterText.text = "" + currentCounterValue;

            // If the currentCounterValue is less than or equal to 0
            if (currentCounterValue <= 0)
            {
                board.currentState = GameState.lose;
                LoseGame();
            }
        }
    }

    public void WinGame()
    {
        youWinPanel.SetActive(true);
        board.currentState = GameState.win;
        currentCounterValue = 0;
        counterText.text = "" + currentCounterValue;
        AnimationManager animManager = FindObjectOfType<AnimationManager>();
        animManager.GameOver();

    }

    public void LoseGame()
    {
        // Set the try again panel to true
        tryAgainPanel.SetActive(true);

        // Set the game state to lose
        board.currentState = GameState.lose; // WHY U NOT WORK????
        // The player has no completed the level
        // Show end level UI
        Debug.Log("Try Again...");
        currentCounterValue = 0;
        counterText.text = "" + currentCounterValue;
        AnimationManager animManager = FindObjectOfType<AnimationManager>();
        animManager.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        // Check foir game type, if it equals time
        if (requirements.gameType == GameType.Time && currentCounterValue > 0)
        {
            // Minus the timerSeconds by time.deltaTime
            timerSeconds -= Time.deltaTime;
            if (timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }
    }
}
