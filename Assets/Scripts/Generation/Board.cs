﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move
}


public enum TileKind
{
    Breakable,
    Blank,
    Normal
}

[System.Serializable]
public class TileType
{
    public int x; // X value in grid
    public int y; // Y value in grid
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    // Setting the current state value (for state machine)
    public GameState currentState = GameState.move;

    // The width of the board
    public int width;
    // The height of the board
    public int height;
    // The offset which the tiles are being created
    public int offSet;
    // The Background tile to be instantiated
    public GameObject backgroundTilePrefab;
    // The shape tiles to be instantiated
    public GameObject[] shapeTiles;

    public TileType[] boardLayout;

    public GameObject destroyEffect;

    // An array of all of the background tiles
    private bool [,] blankSpaces;
    // An array of all of the shape tiles
    public GameObject[,] allShapeTiles;

    public ShapeTile currentTile;

    private FindMatches findMatches;

    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        blankSpaces = new bool [width, height];
        allShapeTiles = new GameObject[width, height];
        // Call the SetUp function
        SetUp();
    }

    public void GenerateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    private void SetUp()
    {
        GenerateBlankSpaces();
        // Cycle through the Width and Height and Instantiate a Tile Prefab at
        // every position
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    // Create a new temporary variable - a Vector2 for the W & H pos
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    // Instantiate the backgroundTilePrefab as a GameObject
                    GameObject backgroundTile = Instantiate(backgroundTilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    // Set the Parent object of the backgroundTile to the Board GameObject
                    backgroundTile.transform.parent = this.transform;
                    // Set the name of each background tile equal to their width and height position on the grid
                    backgroundTile.name = " background tile ( " + i + " , " + j + " )";
                    // choose a random number between 0 and the amount of shapeTiles in the array
                    int tileToUse = Random.Range(0, shapeTiles.Length);

                    // make sure the while loop doesnt become infinite
                    int maxIterations = 0;

                    // Check to see if there are matches, if there are, choose a different shape tile
                    while (MatchesAt(i, j, shapeTiles[tileToUse]) && maxIterations < 100)
                    {
                        // Choose a new tile to instantiate
                        tileToUse = Random.Range(0, shapeTiles.Length);
                        // Increase the maxIterations by 1
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    // Reset maxIterations
                    maxIterations = 0;

                    // Instantiate a random tile from the array (based off the tileToUse result) at this shapeTiles position with no rotation
                    GameObject shapeTile = Instantiate(shapeTiles[tileToUse], tempPosition, Quaternion.identity) as GameObject;

                    // get the ShapeTile script on the tile and set the row to j
                    shapeTile.GetComponent<ShapeTile>().row = j;
                    // get the ShapeTile script on the tile and set the column to i
                    shapeTile.GetComponent<ShapeTile>().column = i;

                    // Set the Parent object of the tile to the BackgroundTile GameObject
                    shapeTile.transform.parent = this.transform;
                    // Set the name of each background tile equal to their width and height position on the grid
                    shapeTile.name = "shape tile ( " + i + " , " + j + " )";

                    allShapeTiles[i, j] = shapeTile;
                }
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        // if column AND row is greater than 1
        if (column > 1 && row > 1)
        {
            if (allShapeTiles[column -1, row] != null && allShapeTiles[column -2, row] != null)
            {
                // if the shape tile 1 and 2 has the same tag as this tile - return true (we've got a match)
                if (allShapeTiles[column - 1, row].tag == piece.tag && allShapeTiles[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }

            if (allShapeTiles[column, row - 1] != null && allShapeTiles[column, row - 2] != null)
            {
                // if the shape tile 1 and 2 has the same tag as this tile - return true (we've got a match)
                if (allShapeTiles[column, row - 1].tag == piece.tag && allShapeTiles[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allShapeTiles[column, row - 1] != null && allShapeTiles[column, row - 2] != null)
                {
                    // Check tiles below
                    if (allShapeTiles[column, row - 1].tag == piece.tag && allShapeTiles[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
            if (column > 1)
            {
                if (allShapeTiles[column - 1, row] != null && allShapeTiles[column - 2, row] != null)
                {
                    // Check tiles below
                    if (allShapeTiles[column - 1, row].tag == piece.tag && allShapeTiles[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }
        
        return false;
    }

    // Check to see how many pieces in list are in one row or column
    private bool ColumnOrRow()
    {
        int numberHorizontal = 0;
        int numberVertical = 0;

        ShapeTile firstPiece = findMatches.currentMatches[0].GetComponent<ShapeTile>();

        if (firstPiece != null)
        {
            foreach (GameObject currentPiece in findMatches.currentMatches)
            {
                ShapeTile tile = currentPiece.GetComponent<ShapeTile>();
                if (tile.row == firstPiece.row)
                {
                    numberHorizontal++;
                }
                if (tile.column == firstPiece.column)
                {
                    numberVertical++;
                }
            }
        }
        return (numberVertical == 5 || numberHorizontal == 5);
    }

    private void CheckToMakeBombs()
    {
        if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
        {
            findMatches.CheckBombs();
        }
        if (findMatches.currentMatches.Count == 5 || findMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                // Make a colour bomb
                // Is the current tile matched?
                // Unmatch it and turn it into a colour bomb
                if (currentTile != null)
                {
                    if (currentTile.isMatched)
                    {
                        if (!currentTile.isColourBomb)
                        {
                            currentTile.isMatched = false;
                            currentTile.MakeColourBomb();
                        }
                    }
                    else {
                        if (currentTile.otherShapeTile != null)
                        {
                            ShapeTile otherTile = currentTile.otherShapeTile.GetComponent<ShapeTile>();
                            if (otherTile.isMatched)
                            {
                                if (!otherTile.isColourBomb)
                                {
                                    otherTile.isMatched = false;
                                    otherTile.MakeColourBomb();
                                }
                            }
                        }
                    }
                }
            } else {
                // Make an adjacent bomb
                // Is the current tile matched?
                // Unmatch it and turn it into a colour bomb
                if (currentTile != null)
                {
                    if (currentTile.isMatched)
                    {
                        if (!currentTile.isAdjacentBomb)
                        {
                            currentTile.isMatched = false;
                            currentTile.MakeAdjacentBomb();
                        }
                    }
                    else
                    {
                        if (currentTile.otherShapeTile != null)
                        {
                            ShapeTile otherTile = currentTile.otherShapeTile.GetComponent<ShapeTile>();
                            if (otherTile.isMatched)
                            {
                                if (!otherTile.isAdjacentBomb)
                                {
                                    otherTile.isMatched = false;
                                    otherTile.MakeAdjacentBomb();
                                }
                            }
                        }
                    }
                }

            }
        }
    }

    // This function handles destroying matches at the correct column and row on the board
    private void DestroyMatchesAt(int column, int row)
    {
        // Destroy matches at position
        // Check if shape is matched, destroy
        if (allShapeTiles[column, row].GetComponent<ShapeTile>().isMatched)
        {
            // How many elements are in the matched pieces list from findmatches?
            if (findMatches.currentMatches.Count >= 4)
            {
                // call the CheckToMakeBombs method
                CheckToMakeBombs();
            }

            // Instantiate Particle effect
            GameObject particle =Instantiate(destroyEffect, allShapeTiles[column, row].transform.position, Quaternion.identity);
            Destroy(particle, 1f);

            // Destroy the correct tile at the column and row position
            Destroy(allShapeTiles[column, row]);
            // Set that coord to null (leaving it empty)
            allShapeTiles[column, row] = null;
        }
    }

    // This method destroys the tiles after they have been matched
    public void DestroyMatches()
    {
        // Cycle through all of the columns
        for (int i = 0; i < width; i++)
        {
            // Cycle through all the rows
            for (int j = 0; j < height; j++)
            {
                if (allShapeTiles[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        findMatches.currentMatches.Clear();
        //StartCoroutine(DecreaseRowCo());
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // If the current spot isn't blank and is empty
                if (!blankSpaces[i, j] && allShapeTiles[i, j] == null)
                {
                    // loop from space above to top of column
                    for (int k = j + 1; k < height; k++)
                    {
                        // if a tile is found
                        if (allShapeTiles[i, k] != null)
                        {
                            // move tile to this empty space
                            allShapeTiles[i, k].GetComponent<ShapeTile>().row = j;
                            // set that spot to be null
                            allShapeTiles[i, k] = null;
                            // break out of the loop
                            break;
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    // Create a coroutine to collapse row
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                
                if (allShapeTiles[i, j] == null)
                {
                    nullCount++;
                }
                // if the null count is greater than 0
                else if (nullCount > 0)
                {
                    // get all of the tiles in the array and decrease their row by the nullCount value
                    allShapeTiles[i, j].GetComponent<ShapeTile>().row -= nullCount;
                    allShapeTiles[i, j] = null;
                }
            }
            nullCount = 0;
        }

        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    // This helper function takes care of refilling the board after a match has been made
    private void RefillBoard()
    {
        // Cycle through the width of the board
        for (int i = 0; i < width; i++)
        {
            // Cycle through the height of the board
            for (int j = 0; j < height; j++)
            {
                // If all of the tiles in the I and J pos are null
                if (allShapeTiles[i, j] == null && !blankSpaces[i, j])
                {
                    // Instantiate a new tile in it's place
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int tileToUse = Random.Range(0, shapeTiles.Length);
                    GameObject piece = Instantiate(shapeTiles[tileToUse], tempPosition, Quaternion.identity);
                    allShapeTiles[i, j] = piece;
                    piece.GetComponent<ShapeTile>().row = j;
                    piece.GetComponent<ShapeTile>().column = i;
                }
            }
        }
    }

    // Check to see if there are anymore matches on the board happening right after a player match
    private bool MatchesOnBoard()
    {
        // Cycle through the width
        for (int i = 0; i < width; i++)
        {
            // Cycle through the height
            for (int j = 0; j < height; j++)
            {
                // If there are tiles in the i and j positions
                if (allShapeTiles[i, j] != null)
                {
                    // Check to see if they are matched, if they are - return TRUE
                    if (allShapeTiles[i, j].GetComponent<ShapeTile>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoardCo()
    {
        // Call the Refill Board function
        RefillBoard();
        // Wait for half a second
        yield return new WaitForSeconds(.5f);

        // While MatchesOnBoard returns true
        while (MatchesOnBoard())
        {
            // Wait for half a second
            yield return new WaitForSeconds(.5f);
            // Then call the DestroyMatches function
            DestroyMatches();
        }
        // Clear the current matches list to prevent any mistaken match 7 column/ row bombs
        findMatches.currentMatches.Clear();
        // Wait for half a second
        yield return new WaitForSeconds(.2f);
        // Set the current game state to move
        currentState = GameState.move;

    }
}
