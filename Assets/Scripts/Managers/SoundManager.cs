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
        // Choose a random number
        int clipToPlay = Random.Range(0, destroyNoise.Length);
        // Play that clip
        destroyNoise[clipToPlay].Play();
    }
}
