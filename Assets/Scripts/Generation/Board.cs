using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // The width of the board
    public int width;
    // The height of the board
    public int height;
    // The Background tile to be instantiated
    public GameObject backgroundTilePrefab;
    // The shape tiles to be instantiated
    public GameObject[] shapeTiles;
    // An array of all of the background tiles
    private BackgroundTile[,] allBGTiles;
    // An array of all of the shape tiles
    public GameObject[,] allShapeTiles;

    void Start()
    {
        allBGTiles = new BackgroundTile[width, height];
        allShapeTiles = new GameObject[width, height];
        // Call the SetUp function
        SetUp();
    }

    private void SetUp()
    {
        // Cycle through the Width and Height and Instantiate a Tile Prefab at
        // every position
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Create a new temporary variable - a Vector2 for the W & H pos
                Vector2 tempPosition = new Vector2(i, j);
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
                }
                // Reset maxIterations
                maxIterations = 0;

                // Instantiate a random tile from the array (based off the tileToUse result) at this shapeTiles position with no rotation
                GameObject shapeTile = Instantiate(shapeTiles[tileToUse], tempPosition, Quaternion.identity) as GameObject;
                // Set the Parent object of the tile to the BackgroundTile GameObject
                shapeTile.transform.parent = this.transform;
                // Set the name of each background tile equal to their width and height position on the grid
                shapeTile.name = "shape tile ( " + i + " , " + j + " )";
                
                allShapeTiles[i, j] = shapeTile;

            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        // if column AND row is greater than 1
        if (column > 1 && row > 1)
        {
            // if the shape tile 1 and 2 has the same tag as this tile - return true (we've got a match)
            if (allShapeTiles[column - 1, row].tag == piece.tag && allShapeTiles[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            // if the shape tile 1 and 2 has the same tag as this tile - return true (we've got a match)
            if (allShapeTiles[column, row - 1].tag == piece.tag && allShapeTiles[column, row - 2].tag == piece.tag)
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                // Check tiles below
                if (allShapeTiles[column, row - 1].tag == piece.tag && allShapeTiles[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                // Check tiles below
                if (allShapeTiles[column -1, row].tag == piece.tag && allShapeTiles[column -2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}
