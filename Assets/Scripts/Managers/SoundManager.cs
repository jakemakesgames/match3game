using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Create references to audio pieces
    public AudioSource[] destroyNoise;

    // Method to play a random destroy noise
    public void PlayRandomDestroyNoise()
    {
        // check what player prefs sound is set at
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 1)
            {
                // Choose a random number
                int clipToPlay = Random.Range(0, destroyNoise.Length);
                // Play that clip
                destroyNoise[clipToPlay].Play();
            }
        }
        else
        {
            // Choose a random number
            int clipToPlay = Random.Range(0, destroyNoise.Length);
            // Play that clip
            destroyNoise[clipToPlay].Play();
        }  
    }
}
