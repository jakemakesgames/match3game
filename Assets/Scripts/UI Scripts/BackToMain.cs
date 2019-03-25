using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinOK()
    {
        // take the current level that we're using, and unlock the next level
        if (gameData != null)
        {
            // unlock the next level
            gameData.saveData.isActive[board.level + 1] = true;
            // save our new data
            gameData.Save();
        }

        SceneManager.LoadScene(sceneToLoad); // load the scene to load
    }

    public void LoseOK()
    {
        SceneManager.LoadScene(sceneToLoad); // load the scene to load
    }
}
