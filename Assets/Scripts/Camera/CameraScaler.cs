using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    private Board board;
    public float cameraOffset;
    public float aspectRatio = 0.625f;
    public float padding = 2;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        // Check to see if the board is not null
        if (board != null)
        {
            // Call the RepositionCamera function, passing in the board's width and height
            RepositionCamera(board.width - 1, board.height - 1);
        }
    }

    // This method handles repositioning the camera
    void RepositionCamera(float x, float y)
    {
        // Create a new temporary position
        Vector3 tempPosition = new Vector3 (x/2, y/2, cameraOffset);
        // Set the camera's position equal to the tempPosition
        transform.position = tempPosition;

        if (board.width >= board.height)
        {
            // Change the orthographic size
            Camera.main.orthographicSize = (board.width / 2 + padding) / aspectRatio;
        }
        else
        {
            Camera.main.orthographicSize = board.height / 2 + padding;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
