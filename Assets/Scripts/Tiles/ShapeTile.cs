using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTile : MonoBehaviour
{
    // The column and row of this ShapeTile
    public int column;
    public int row;

    // Ints to *actually* move the pieces around
    public int targetX;
    public int targetY;

    // A reference to the Board Object
    private Board board;

    private GameObject otherShapeTile;

    // Handling Player Touch positions
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0.0f;

    public float tileMoveSpeed;

    void Start()
    {
        // Set the board component equal to the GameObject in the scene with the Board script attached
        board = FindObjectOfType<Board>();

        // Set the targetX and targetY values as this GameObjects X and Y positions (cast into a int)
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        // Set the row and column
        column = targetX;
        row = targetY;
    }

    void Update()
    {
        targetX = column;
        targetY = row;

        // Horizontal Swiping
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            // Move Towards Target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, tileMoveSpeed);
        }
        else
        {
            // Directly Set Position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allShapeTiles[column, row] = this.gameObject;
        }

        // Vertical Swiping
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move Towards Target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, tileMoveSpeed);
        }
        else
        {
            // Directly Set Position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allShapeTiles[column, row] = this.gameObject;
        }
    }

    private void OnMouseDown()
    {
        // Set the firstTouchPosition value equal to the mousePosition (ScreenToWorldPoint)
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    private void OnMouseUp()
    {
        // Set the finalTouchPosition value equal to the mousePosition (ScreenToWorldPoint)
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Call the CalculateAngle function
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180/ Mathf.PI;
        Debug.Log(swipeAngle);
        // Call the MovePieces function
        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            // Right Swipe
            // Grab the shapeTile to the right of this tile and plus 1 to its column
            otherShapeTile = board.allShapeTiles[column + 1, row];
            // Get the shapeTile script and subtract 1 off its column
            otherShapeTile.GetComponent<ShapeTile>().column -= 1;
            // Set this Tiles colum to +1
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height )
        {
            // Up Swipe
            // Grab the shapeTile to the right of this tile and plus 1 to its column
            otherShapeTile = board.allShapeTiles[column, row + 1];
            // Get the shapeTile script and subtract 1 off its column
            otherShapeTile.GetComponent<ShapeTile>().row -= 1;
            // Set this Tiles colum to +1
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            // Left Swipe
            // Grab the shapeTile to the right of this tile and plus 1 to its column
            otherShapeTile = board.allShapeTiles[column - 1, row];
            // Get the shapeTile script and subtract 1 off its column
            otherShapeTile.GetComponent<ShapeTile>().column += 1;
            // Set this Tiles colum to +1
            column -= 1;
        }
        else  if (swipeAngle < -45 && swipeAngle >= 135 && row > 0)
        {
            // Down Swipe
            // Grab the shapeTile to the right of this tile and plus 1 to its column
            otherShapeTile = board.allShapeTiles[column, row -1];
            // Get the shapeTile script and subtract 1 off its column
            otherShapeTile.GetComponent<ShapeTile>().row += 1;
            // Set this Tiles colum to +1
            row -= 1;
        }
    }
}
