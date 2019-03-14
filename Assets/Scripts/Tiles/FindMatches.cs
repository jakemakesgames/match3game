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
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentTile = board.allShapeTiles[i, j];
                if (currentTile != null)
                {
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftTile = board.allShapeTiles[i - 1, j];
                        GameObject rightTile = board.allShapeTiles[i + 1, j];
                        if (leftTile != null && rightTile != null)
                        {
                            if (leftTile.tag == currentTile.tag && rightTile.tag == currentTile.tag)
                            {
                                leftTile.GetComponent<ShapeTile>().isMatched = true;
                                rightTile.GetComponent<ShapeTile>().isMatched = true;
                                currentTile.GetComponent<ShapeTile>().isMatched = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upTile = board.allShapeTiles[i, j + 1];
                        GameObject downTile = board.allShapeTiles[i, j - 1];
                        if (upTile != null && downTile != null)
                        {
                            if (upTile.tag == currentTile.tag && downTile.tag == currentTile.tag)
                            {
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
