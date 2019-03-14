using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
                                if (currentTile.GetComponent<ShapeTile>().isRowBomb || leftTile.GetComponent<ShapeTile>().isRowBomb || rightTile.GetComponent<ShapeTile>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));
                                }

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

    // Helper method - Column Pieces
    List<GameObject> GetColumnPieces(int column)
    {
        // Initialize list
        List<GameObject> tiles = new List<GameObject>();
        // Cycle through tiles in array and see if they're in this list
        for (int i = 0; i < board.height; i++)
        {
            if (board.allShapeTiles[column, i] != null)
            {
                tiles.Add(board.allShapeTiles[column, i]);
                board.allShapeTiles[column, i].GetComponent<ShapeTile>().isMatched = true;
            }
        }
        return tiles;
    }

    // Helper method - Row Pieces
    List<GameObject> GetRowPieces(int row)
    {
        // Initialize list
        List<GameObject> tiles = new List<GameObject>();
        // Cycle through tiles in array and see if they're in this list
        for (int i = 0; i < board.width; i++)
        {
            if (board.allShapeTiles[i, row] != null)
            {
                tiles.Add(board.allShapeTiles[i, row]);
                board.allShapeTiles[i, row].GetComponent<ShapeTile>().isMatched = true;
            }
        }
        return tiles;
    }
}
