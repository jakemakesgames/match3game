using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elements")]
    public bool isActive; // is the current level locked or not?
    public Sprite activeSprite; // the sprite shown if the level is active
    public Sprite lockedSprite; // the sprite shown if the level is locked
    public Image buttonImage; // the default image for the button
    private Button myButton; // get a reference to the button
    [Space(5)]
    public Image[] stars; // get a reference to the stars
    public Text levelNumberText; // get a reference to the level number text
    public int level; // keep track of what level this belongs to
    public GameObject confirmPanel; // get a reference to the confirm panel

    // Start is called before the first frame update
    void Start()
    {
        // complete the reference to the button image
        buttonImage = GetComponent<Image>();
        // complete the reference to the button component
        myButton = GetComponent<Button>();
        // call the ShowLevel method
        ShowLevel();
        // call the ActivateStars method
        ActivateStars();
        // call the DecideSprite method
        DecideSprite();
    }

    // activate or deativate the stars according to what the player has achieved
    void ActivateStars()
    {
        //  -> COME BACK TO THIS LATER <- \\
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = false;
        }
    }

    // method to give button the correct sprite
    void DecideSprite()
    {
        // if the button is active, display the correct sprite and set the button to be interactable
        if (isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelNumberText.enabled = true;
        }
        // if the button is not active, do the opposite
        else
        {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelNumberText.enabled = false;
        }
    }

    // show the correct level text
    void ShowLevel()
    {
        levelNumberText.text = "" + level;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Sets the confirm panel to true
    public void ConfirmPanel(int level)
    {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }
}
