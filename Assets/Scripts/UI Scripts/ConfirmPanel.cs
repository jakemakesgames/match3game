using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    [Header("Level Information")]
    public string levelToLoad; // what level will load
    public int level; // which level will we be confirming
    private GameData gameData;

    private int starsActive;
    private int hiScore;

    [Header("UI")]
    public Image[] stars; // get a reference to the stars
    //public Text highScoreText;
    //public Text starText;
    public TMP_Text highScoreText;
    public TMP_Text starText;

    // Start is called before the first frame update
    void OnEnable()
    {
        gameData = FindObjectOfType<GameData>();

        LoadData();

        // call the ActivateStars method
        ActivateStars();
        SetText();
    }

    void LoadData()
    {
        if (gameData != null)
        {
            starsActive = gameData.saveData.stars[level - 1];
            hiScore = gameData.saveData.highScores[level - 1];
        }
    }

    void SetText()
    {
        highScoreText.text = "" + hiScore;
        starText.text = "" + starsActive + "/3";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // activate or deativate the stars according to what the player has achieved
    void ActivateStars()
    {
        //  -> COME BACK TO THIS LATER <- \\
        for (int i = 0; i < starsActive; i++)
        {
            stars[i].enabled = true;
        }
    }

    // closing confirm panel
    public void Cancel ()
    {
        this.gameObject.SetActive(false);
    }

    // loading scene
    public void PlayLevel()
    {
        PlayerPrefs.SetInt("Current Level", level - 1);

        SceneManager.LoadScene(levelToLoad);
    }
}
