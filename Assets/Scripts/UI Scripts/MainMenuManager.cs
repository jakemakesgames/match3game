using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager menuManager;

    public Animator anim;

    public void PlayGame()
    {
        anim.SetBool("Play", true);
        anim.SetBool("Quit", false);
    }

    public void QuitGame()
    {
        anim.SetBool("Quit", true);
        anim.SetBool("Play", false);
    }
}
