using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
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
                // Instantiate the tilePrefab as a GameObject
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                // Set the Parent object of the backgroundTile to the Board GameObject
                backgroundTile.transform.parent = this.transform;
                // Set the name of each background tile equal to their width and height position on the grid
                backgroundTile.name = "( " + i + " , " + j + " )";

            }
        }
    }
}
