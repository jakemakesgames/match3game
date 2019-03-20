using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator fadePanelAnim;
    public Animator goalPanelAnim;

    public void OK()
    {
        if (fadePanelAnim != null && goalPanelAnim != null)
        {
            // Trigger both "Out" bools to true
            fadePanelAnim.SetBool("Out", true);
            goalPanelAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
    }

    public void GameOver()
    {
        fadePanelAnim.SetBool("Out", false);
        fadePanelAnim.SetBool("GameOver", true);
    }

    IEnumerator GameStartCo()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);
        // Find the board
        Board board = FindObjectOfType<Board>();
        // Set the game state
        board.currentState = GameState.move;
    }
}
