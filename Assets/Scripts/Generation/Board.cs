using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject backgroundTilePrefab;
    public GameObject[] tiles;
    private BackgroundTile[,] allTiles;

    void Start()
    {
        // Tell the allTiles arrway how big it needs to be 
        allTiles = new BackgroundTile[width, height];
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
                // choose a random number between 0 and the amount of tiles in the array
                int tileToUse = Random.Range(0, tiles.Length);
                // Instantiate a random tile from the array (based off the tileToUse result) at this tiles position with no rotation
                GameObject tile = Instantiate(tiles[tileToUse], tempPosition, Quaternion.identity) as GameObject;
                // Set the Parent object of the tile to the BackgroundTile GameObject
                tile.transform.parent = this.transform;
                // Set the name of each background tile equal to their width and height position on the grid
                tile.name = "shape tile ( " + i + " , " + j + " )";

            }
        }
    }
}
