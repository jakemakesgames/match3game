using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public EndGameRequirements requirements; // Reference EndGameRequirements
    public GameObject movesLabel;
    public GameObject timeLabel;
    public Text counterText;
    public int currentCounterValue;
    private float timerSeconds;

    // Start is called before the first frame update
    void Start()
    {
        // Call the SetUpGame function
        SetUpGame();
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
        // Decrease the value, update the text
        currentCounterValue--;
        counterText.text = "" + currentCounterValue;

        if (currentCounterValue <= 0)
        {
            Debug.Log("Try Again...");
            currentCounterValue = 0;
            counterText.text = "" + currentCounterValue;
        }
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
