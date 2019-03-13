using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    // Get a reference to the board
    private Board board;
    // Create a list of the current matches
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();
    }

    public void FindAllMatches()
    {
        // Start the Find All Matches Coroutine
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // Find current tile
                GameObject currentTile = board.allShapeTiles[i, j];
                // Make sure the space isn't empty
                if (currentTile != null)
                {
                    // LEFT AND RIGHT MATCHES
                    if (i > 0 && i < board.width - 1)
                    {
                        // Create a tile to the left of this 
                        GameObject leftTile = board.allShapeTiles[i - 1, j];
                        // Create a tile to the right of this
                        GameObject rightTile = board.allShapeTiles[i + 1, j];
                        // Check to see if either left or right tile is NOT null
                        if (leftTile != null && rightTile != null)
                        {
                            // Check to see if the tags of the left and right tile are equal to this one
                            if (leftTile.tag == currentTile.tag && rightTile.tag == currentTile.tag)
                            {
                                // If the currentMatches list does not contain the left tile
                                if (!currentMatches.Contains(leftTile))
                                {
                                    // Add the left tile to the list
                                    currentMatches.Add(leftTile);
                                }
                                // If the currentMatches list does not contain the right tile
                                if (!currentMatches.Contains(rightTile))
                                {
                                    // Add the right tile to the list
                                    currentMatches.Add(rightTile);
                                }
                                // If the currentMatches list does not contain the current tile
                                if (!currentMatches.Contains(currentTile))
                                {
                                    // Add the current tile to the list
                                    currentMatches.Add(currentTile);
                                }

                                // Set the left, right and current tiles to isMatched = true
                                leftTile.GetComponent<ShapeTile>().isMatched = true;
                                rightTile.GetComponent<ShapeTile>().isMatched = true;
                                currentTile.GetComponent<ShapeTile>().isMatched = true;
                            }
                        }
                        
                    }

                    // UP AND DOWN MATCHES
                    if (j > 0 && j < board.height - 1)
                    {
                        // Create a tile above this
                        GameObject upTile = board.allShapeTiles[i, j + 1];
                        // Create a tile below this
                        GameObject downTile = board.allShapeTiles[i, j - 1];
                        // Check to see if either up or down tile is NOT null
                        if (upTile != null && downTile != null)
                        {
                            // Check to see if the tags of the up and down tile are equal to this one
                            if (upTile.tag == currentTile.tag && downTile.tag == currentTile.tag)
                            {
                                // If the currentMatches list does not contain the up tile
                                if (!currentMatches.Contains(upTile))
                                {
                                    // Add the up tile to the list
                                    currentMatches.Add(upTile);
                                }
                                // If the currentMatches list does not contain the down tile
                                if (!currentMatches.Contains(downTile))
                                {
                                    // Add the down tile to the list
                                    currentMatches.Add(downTile);
                                }
                                // If the currentMatches list does not contain the current tile
                                if (!currentMatches.Contains(currentTile))
                                {
                                    // Add the current tile to the list
                                    currentMatches.Add(currentTile);
                                }
                                // Set the up, down and current tiles to isMatched = true
                                upTile.GetComponent<ShapeTile>().isMatched = true;
                                downTile.GetComponent<ShapeTile>().isMatched = true;
                                currentTile.GetComponent<ShapeTile>().isMatched = true;
                            }
                        }

                    }
                }
            }
        }
    }
    
}
