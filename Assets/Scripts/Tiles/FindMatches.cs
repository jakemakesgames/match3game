﻿using System.Collections;
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
                            // If the left and right tags are equal to the current tiles tag
                            if (leftTile.tag == currentTile.tag && rightTile.tag == currentTile.tag)
                            {
                                // If either the left, right or current tiles are row bombs
                                if (currentTile.GetComponent<ShapeTile>().isRowBomb || leftTile.GetComponent<ShapeTile>().isRowBomb || rightTile.GetComponent<ShapeTile>().isRowBomb)
                                {
                                    // Do the thing
                                    currentMatches.Union(GetRowPieces(j));
                                }

                                // Check to see if the current, left or right tiles are also column bombs
                                if (currentTile.GetComponent<ShapeTile>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i));
                                }

                                // Check to see if the left edge tile is a column bomb
                                if (leftTile.GetComponent<ShapeTile>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i - 1));
                                }

                                // Check to see if the right edge tile is a column bomb
                                if (rightTile.GetComponent<ShapeTile>().isColumnBomb)
                                {
                                    currentMatches.Union(GetColumnPieces(i + 1));
                                }

                                // ADD THE TILE TO THE LIST IF IT'S NOT ALREADY IN IT
                                if (!currentMatches.Contains(leftTile))
                                {
                                    currentMatches.Add(leftTile);
                                }
                                leftTile.GetComponent<ShapeTile>().isMatched = true;
                                // ADD THE TILE TO THE LIST IF IT'S NOT ALREADY IN IT
                                if (!currentMatches.Contains(rightTile))
                                {
                                    currentMatches.Add(rightTile);
                                }
                                rightTile.GetComponent<ShapeTile>().isMatched = true;
                                // ADD THE TILE TO THE LIST IF IT'S NOT ALREADY IN IT
                                if (!currentMatches.Contains(currentTile))
                                {
                                    currentMatches.Add(currentTile);
                                }
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
                                // If either the left, right or current tiles are column bombs
                                if (currentTile.GetComponent<ShapeTile>().isColumnBomb || upTile.GetComponent<ShapeTile>().isColumnBomb || downTile.GetComponent<ShapeTile>().isColumnBomb)
                                {
                                    // Do the thing
                                    currentMatches.Union(GetColumnPieces(i));
                                }

                                // Check to see if the middle piece is a row bomb
                                if (currentTile.GetComponent<ShapeTile>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j));
                                }

                                // Check to see if the up piece is a row bomb
                                if (upTile.GetComponent<ShapeTile>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j + 1));
                                }

                                // Check to see if the down piece is a row bomb
                                if (downTile.GetComponent<ShapeTile>().isRowBomb)
                                {
                                    currentMatches.Union(GetRowPieces(j - 1));
                                }

                                // ADD THE TILE TO THE LIST IF IT'S NOT ALREADY IN IT
                                if (!currentMatches.Contains(upTile))
                                {
                                    currentMatches.Add(upTile);
                                }
                                upTile.GetComponent<ShapeTile>().isMatched = true;
                                // ADD THE TILE TO THE LIST IF IT'S NOT ALREADY IN IT
                                if (!currentMatches.Contains(downTile))
                                {
                                    currentMatches.Add(downTile);
                                }
                                downTile.GetComponent<ShapeTile>().isMatched = true;
                                // ADD THE TILE TO THE LIST IF IT'S NOT ALREADY IN IT
                                if (!currentMatches.Contains(currentTile))
                                {
                                    currentMatches.Add(currentTile);
                                }
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
