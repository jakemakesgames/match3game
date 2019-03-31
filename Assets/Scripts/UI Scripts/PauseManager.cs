using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pausePanel;
    private Board board;
    public bool paused = false;

    public Image soundButton;
    public Sprite soundOn;
    public Sprite soundOff;
    public TMP_Text soundText;

    void Start()
    {
        // in player prefs the "Sound" key is for sound
        // if sound = 0 -> mute | if sound = 1 -> unmute 
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = soundOff;
                soundText.text = "Off".ToString();
            }
            else
            {
                soundButton.sprite = soundOn;
                soundText.text = "On".ToString();
            }

        }
        else
        {
            soundButton.sprite = soundOn;
        }


        pausePanel.SetActive(false);
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>() ;
    }

    void Update()
    {
        if (paused == true && !pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(true);
            board.currentState = GameState.pause;
        }
        if (paused == false && pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            board.currentState = GameState.move;
        }
    }

    public void SoundButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.sprite = soundOn;
                soundText.text = "On".ToString();
                PlayerPrefs.SetInt("Sound", 1);
            }
            else
            {
                soundButton.sprite = soundOff;
                soundText.text = "Off".ToString();
                PlayerPrefs.SetInt("Sound", 0);
            }
        }
        else
        {
            soundButton.sprite = soundOff;
        }
    }

    public void PauseGame()
    {
        paused = !paused;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
