using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class SaveData
{
    public bool[] isActive; // is the level active
    public int[] highScores; // highscores the player has got
    public int[] stars; // how many stars does the player have
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;

    void Awake()
    {
        // Singleton Pattern
        // check to see if gameData is null
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        } else
        {
            Destroy(this.gameObject);
        }

        // call the load method
        Load();
    }

    // this method takes care of saving the data once the player has closed the application
    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnDisable()
    {
        // call the Save method
        Save();
    }

    public void Save()
    {
        // create a binary formatter which can read binary files
        BinaryFormatter formatter = new BinaryFormatter();

        // create a route from the program to the file
        FileStream file = File.Open(Application.persistentDataPath + "/player.data", FileMode.Create);

        // create new player data
        SaveData data = new SaveData();
        data = saveData;

        // do the saving
        formatter.Serialize(file, data);

        // close the file
        file.Close();

        // debug log 
        Debug.Log("Saved!");
    }

    public void Load()
    {
        // check if the save game file exists
        if (File.Exists(Application.persistentDataPath + "/player.data"))
        {
            // create a binary formatter
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/player.data", FileMode.Open);
            // change player data
            saveData = formatter.Deserialize(file) as SaveData;
            // close file
            file.Close();
            // debug log
            Debug.Log("Loaded!");
        }
        else
        {
            // if the file doesn't exist - initialize it
            saveData = new SaveData();
            saveData.isActive = new bool [100]; // the values set here are the amount of levels in the game etc
            saveData.stars = new int [100];
            saveData.highScores = new int [100];
            saveData.isActive[0] = true; // set the first level in the array true (so the first level will be unlocked)

        }
        
    }
}
