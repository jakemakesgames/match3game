using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintManager : MonoBehaviour
{
    private Board board;
    public float hintDelay;
    private float hintDelaySeconds;
    public GameObject hintParticle;
    public GameObject currentHint;

    void Start()
    {
        board = FindObjectOfType<Board>(); // Get reference to board
        hintDelaySeconds = hintDelay;
    }

    void Update()
    {
        // Start the countdown
        hintDelaySeconds -= Time.deltaTime;
        // If hintDelaySeconds is less than or equal to zero AND there is no current hiny
        if (hintDelaySeconds <= 0 && currentHint == null)
        {
            // Call the MarkHint method
            MarkHint();
            // Reset the timer
            hintDelaySeconds = hintDelay;
        }

    }

    // Want to find all possible matches on board
    List<GameObject> FindAllMatches()
    {
        List<GameObject> possibleMoves = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allShapeTiles[i, j] != null)
                {
                    if (i < board.width - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.right))
                        {
                            possibleMoves.Add(board.allShapeTiles[i, j]);
                        }
                    }
                    if (j < board.height - 1)
                    {
                        if (board.SwitchAndCheck(i, j, Vector2.up))
                        {
                            possibleMoves.Add(board.allShapeTiles[i, j]);
                        }
                    }
                }
            }
        }
        return possibleMoves;
    }
    // Pick one of those matches randomly
    GameObject PickOneRandomly()
    {
        // create a new list of gameObjects called possibleMoves
        List<GameObject> possibleMoves = new List<GameObject>();
        // Set possibleMoves equal to FindAllMatches
        possibleMoves = FindAllMatches();
        // Check for matches
        if (possibleMoves.Count > 0)
        {
            // Set pieceToUse to a random number within range
            int pieceToUse = Random.Range(0, possibleMoves.Count);
            // Return the piece to use
            return possibleMoves[pieceToUse];
        }
        // Else return null
        return null;
    }
    // Create the hint behind the chosen match
    private void MarkHint()
    {
        // Set move equal to PickOneRandomly
        GameObject move = PickOneRandomly();
        // If move is not null
        if (move != null)
        {
            // Instantiate the hint particle at the move position
            currentHint = Instantiate(hintParticle, move.transform.position, Quaternion.identity);
        }
    }

    // Destroy the hint marker
    public void DestroyHint()
    {
        // If the currentHint is not equal to null
        if (currentHint != null)
        {
            // Destroy the current hint
            Destroy(currentHint);
            // Set currentHint to null
            currentHint = null;
            // Reset the hintDelay timer
            hintDelaySeconds = hintDelay;
        }
    }
}
