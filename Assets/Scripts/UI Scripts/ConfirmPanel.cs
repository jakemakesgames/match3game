using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{
    public string levelToLoad; // what level will load
    public Image[] stars; // get a reference to the stars
    public int level; // which level will we be confirming

    // Start is called before the first frame update
    void Start()
    {
        // call the ActivateStars method
        ActivateStars();
    }

    // Update is called once per frame
    void Update()
    {
        
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
