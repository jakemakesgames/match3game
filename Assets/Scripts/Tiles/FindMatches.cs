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

    private void AddToListAndMatch(GameObject tile)
    {
        if (!currentMatches.Contains(tile))
        {
            currentMatches.Add(tile);
        }
        tile.GetComponent<ShapeTile>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject tile1, GameObject tile2, GameObject tile3)
    {
        AddToListAndMatch(tile1);
        AddToListAndMatch(tile2);
        AddToListAndMatch(tile3);
    }

    // Helper method to find Adjacent bombs
    private List<GameObject> IsAdjacentBomb(ShapeTile tile1, ShapeTile tile2, ShapeTile tile3)
    {
        List<GameObject> currentTiles = new List<GameObject>();
        // Check to see if the middle piece is an adjacent bomb
        if (tile1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(tile1.column, tile1.row));
        }

        // Check to see if the up piece is an adjacent bomb
        if (tile2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(tile2.column, tile2.row));
        }

        // Check to see if the down piece is an adjacent bomb
        if (tile3.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(tile3.column, tile3.row));
        }
        return currentTiles;
    }


    // Helper method to find RowBombs
    private List<GameObject> IsRowBomb(ShapeTile tile1, ShapeTile tile2, ShapeTile tile3)
    {
        List<GameObject> currentTiles = new List<GameObject>();
        // Check to see if the middle piece is a row bomb
        if (tile1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(tile1.row));
        }

        // Check to see if the up piece is a row bomb
        if (tile2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(tile2.row));
        }

        // Check to see if the down piece is a row bomb
        if (tile3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(tile3.row));
        }
        return currentTiles;
    }

    // Helper method to find ColumnBombs
    private List<GameObject> IsColumnBomb(ShapeTile tile1, ShapeTile tile2, ShapeTile tile3)
    {
        List<GameObject> currentTiles = new List<GameObject>();
        // Check to see if the middle piece is a row bomb
        if (tile1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(tile1.column));
        }

        // Check to see if the up piece is a row bomb
        if (tile2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(tile2.column));
        }

        // Check to see if the down piece is a row bomb
        if (tile3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(tile3.column));
        }
        return currentTiles;
    }


    private IEnumerator FindAllMatchesCo()
    {
        //yield return new WaitForSeconds(.1f);
        yield return null; 
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentTile = board.allShapeTiles[i, j];
                if (currentTile != null)
                {
                    ShapeTile currentTileTile = currentTile.GetComponent<ShapeTile>();
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftTile = board.allShapeTiles[i - 1, j];
                        GameObject rightTile = board.allShapeTiles[i + 1, j];

                        if (leftTile != null && rightTile != null)
                        {
                            ShapeTile rightTileTile = leftTile.GetComponent<ShapeTile>();
                            ShapeTile leftTileTile = leftTile.GetComponent<ShapeTile>();

                            if (leftTile != null && rightTile != null)
                            {
                                // If the left and right tags are equal to the current tiles tag
                                if (leftTile.tag == currentTile.tag && rightTile.tag == currentTile.tag)
                                {
                                    // Check for row bombs
                                    currentMatches.Union(IsRowBomb(leftTileTile, currentTileTile, rightTileTile));
                                    // Check for column bombs
                                    currentMatches.Union(IsColumnBomb(leftTileTile, currentTileTile, rightTileTile));
                                    // Check for Adjacent bombs
                                    currentMatches.Union(IsAdjacentBomb(leftTileTile, currentTileTile, rightTileTile));

                                    GetNearbyPieces(leftTile, currentTile, rightTile);
                                }
                            }
                        }
                    }
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upTile = board.allShapeTiles[i, j + 1];
                        GameObject downTile = board.allShapeTiles[i, j - 1];

                        if (upTile != null && downTile != null)
                        {
                            ShapeTile downTileTile = downTile.GetComponent<ShapeTile>();
                            ShapeTile upTileTile = upTile.GetComponent<ShapeTile>();

                            if (upTile != null && downTile != null)
                            {
                                if (upTile.tag == currentTile.tag && downTile.tag == currentTile.tag)
                                {
                                    // Check for column bombs
                                    currentMatches.Union(IsColumnBomb(upTileTile, currentTileTile, downTileTile));
                                    // Check for row bombs
                                    currentMatches.Union(IsRowBomb(upTileTile, currentTileTile, downTileTile));
                                    // Check for Adjacent bombs
                                    currentMatches.Union(IsAdjacentBomb(upTileTile, currentTileTile, downTileTile));

                                    GetNearbyPieces(upTile, currentTile, downTile);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Find all tiles of a colour and turn it to is matched
    public void MatchPiecesOfColour(string colour)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                // Check if that piece exists
                if (board.allShapeTiles[i, j] != null)
                {
                    // Check the tag on that tile
                    if (board.allShapeTiles[i, j].tag == colour)
                    {
                        // Set that dot to be matched
                        board.allShapeTiles[i, j].GetComponent<ShapeTile>().isMatched = true;
                    }
                }
            }
        }
    }

    // A helper method for the adjacent pieces
    List<GameObject> GetAdjacentPieces(int column, int row)
    {
        List<GameObject> tiles = new List<GameObject>();
        // Loop thtough the rows and columns 1 directly in each dir
        for (int i = column -1; i <= column + 1; i++)
        {
            for (int j  = row - 1; j  <= row + 1; j ++)
            {
                // Check to see if the piece is inside the board
                if(i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    if (board.allShapeTiles[i, j] != null)
                    {
                        tiles.Add(board.allShapeTiles[i, j]);
                        board.allShapeTiles[i, j].GetComponent<ShapeTile>().isMatched = true;
                    }
                }
            }
        }
        return tiles;
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
                ShapeTile tile = board.allShapeTiles[column, i].GetComponent<ShapeTile>();

                // Check to see if one of the tiles happens to be a row bomb, if so -> union those pieces to the tiles list
                if (tile.isRowBomb)
                {
                    tiles.Union(GetRowPieces(i)).ToList();
                }

                tiles.Add(board.allShapeTiles[column, i]);
                tile.isMatched = true;
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
                ShapeTile tile = board.allShapeTiles[i, row].GetComponent<ShapeTile>();

                // Check to see if one of the tiles happens to be a column bomb, if so -> union those pieces to the tiles list
                if (tile.isColumnBomb)
                {
                    tiles.Union(GetColumnPieces(i)).ToList();
                }

                tiles.Add(board.allShapeTiles[i, row]);
                tile.isMatched = true;
            }
        }
        return tiles;
    }

    // Check to make bombs
    public void CheckBombs()
    {
        // Did the player move something
        if (board.currentTile != null)
        {
            // Is the piece they moved matched?
            if (board.currentTile.isMatched)
            {
                // Make it unmatched
                board.currentTile.isMatched = false;

                // Right Swipe
                if ((board.currentTile.swipeAngle > -45 && board.currentTile.swipeAngle <= 45)
                    // Left Swipe
                    || board.currentTile.swipeAngle < -135 || board.currentTile.swipeAngle >= 135)
                {
                    // MAKE ROW BOMB
                    board.currentTile.MakeRowBomb();
                }
                // else its an up or down swipe
                else
                {
                    // MAKE COLUMN BOMB
                    board.currentTile.MakeColumnBomb();
                }

                #region OLD BOMB CODE
                // Decide which bomb we're going to place
                /*
                int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 50)
                {
                    // Make a row bomb
                    board.currentTile.MakeRowBomb();
                }
                else if (typeOfBomb >= 50)
                {
                    // Make a column bomb
                    board.currentTile.MakeColumnBomb();
                }
                */
                #endregion

            }
            // Is the other piece matched?
            else if (board.currentTile.otherShapeTile != null)
            {
                ShapeTile otherTile = board.currentTile.otherShapeTile.GetComponent<ShapeTile>();
                if (otherTile.isMatched)
                {
                    otherTile.isMatched = false;

                    // Right Swipe
                    if ((board.currentTile.swipeAngle > -45 && board.currentTile.swipeAngle <= 45)
                        // Left Swipe
                        || board.currentTile.swipeAngle < -135 || board.currentTile.swipeAngle >= 135)
                    {
                        // MAKE ROW BOMB
                        otherTile.MakeRowBomb();
                    }
                    // else its an up or down swipe
                    else
                    {
                        // MAKE COLUMN BOMB
                        otherTile.MakeColumnBomb();
                    }
                }
            }
        }
    }
}
