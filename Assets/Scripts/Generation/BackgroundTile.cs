using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] tiles;

    
    void Start()
    {
        // Call the Initialize function
        Initialize();
    }

    void Initialize()
    {
        // choose a random number between 0 and the amount of tiles in the array
        int tileToUse = Random.Range(0, tiles.Length);
        // Instantiate a random tile from the array (based off the tileToUse result) at this tiles position with no rotation
        GameObject tile = Instantiate(tiles[tileToUse], transform.position, Quaternion.identity) as GameObject;
        // Set the Parent object of the tile to the BackgroundTile GameObject
        tile.transform.parent = this.transform;
        // Set the name of each background tile equal to their width and height position on the grid
        tile.name = this.gameObject.name;
    }
}
