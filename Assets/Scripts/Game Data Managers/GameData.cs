using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// game save data class
[Serializable]
public class SaveData
{
    public bool[] isActive; // is the level active
    public int[] highScores; // what are the high scores
    public int[] stars; // how many stars does the player have?
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;
    public SaveData saveData;
   
    void Awake()
    {
        // check to see if gameData is null
        if (gameData == null)
        {
            DontDestroyOnLoad(this.gameObject);
            gameData = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
    }

    public void Save()
    {
        // create a binary formatter which can read binary files
        BinaryFormatter formatter = new BinaryFormatter();

        // create a route from the program to the file
        FileStream file = File.Open(Application.persistentDataPath + "/player.dat", FileMode.Open);

        // create a copy of save data
        SaveData data = new SaveData();
        data = saveData;

        // do the saving - take formatter and serialise
        formatter.Serialize(file, data);

        // close the file
        file.Close();
        Debug.Log("Saved");
    }

    public void Load()
    {

    }

    private void OnDisable()
    {
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
