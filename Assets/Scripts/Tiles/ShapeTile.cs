using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTile : MonoBehaviour
{
    [Header("Board Variables")] 
    public int column; // The Colomn Coordinate of this Tile
    public int row; // The Row Coordinate of this Tile
    public int previousColumn; // The previous Column Coordinate
    public int previousRow; // The previous Row Coordinate
    // Ints to *actually* move the pieces around
    public int targetX;
    public int targetY;
    // Check if it's matched
    public bool isMatched = false;
    // A reference to the Board Object
    private Board board;
    private GameObject otherShapeTile;

    // Handling Player Touch positions
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0.0f;
    public float swipeResist = 1f;

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
        previousRow = row;
        previousColumn = column;
    }

    void Update()
    {
        FindMatches();
        if (isMatched)
        {
            // if this tile IS matched, grey out the colour of the sprite
            SpriteRenderer mySprite = GetComponentInChildren<SpriteRenderer>();
            mySprite.color = new Color(1, 1, 1, 0.2f);
        }

        targetX = column;
        targetY = row;

        #region Horizontal Swiping
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            // Move Towards Target
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, tileMoveSpeed);
            if (board.allShapeTiles[column, row] != this.gameObject)
            {
                board.allShapeTiles[column, row] = this.gameObject;
            }
        }
        else
        {
            // Directly Set Position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        #endregion

        #region Vertical Swiping
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            // Move Towards Target
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, tileMoveSpeed);
            if (board.allShapeTiles[column, row] != this.gameObject)
            {
                board.allShapeTiles[column, row] = this.gameObject;
            }
        }
        else
        {
            // Directly Set Position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }
        #endregion
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
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            // Debug.Log(swipeAngle);
            // Call the MovePieces function
            MovePieces();
        }  
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width -1)
        {
            // Right Swipe
            // Grab the shapeTile to the right of this tile and plus 1 to its column
            otherShapeTile = board.allShapeTiles[column + 1, row];
            // Get the shapeTile script and subtract 1 off its column
            otherShapeTile.GetComponent<ShapeTile>().column -= 1;
            // Set this Tiles colum to +1
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height -1)
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
        else  if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // Down Swipe
            // Grab the shapeTile to the right of this tile and plus 1 to its column
            otherShapeTile = board.allShapeTiles[column, row -1];
            // Get the shapeTile script and subtract 1 off its column
            otherShapeTile.GetComponent<ShapeTile>().row += 1;
            // Set this Tiles colum to +1
            row -= 1;
        }
        // Start the CheckMoveCo coroutine
        StartCoroutine(CheckMoveCo());
    }

    public IEnumerator CheckMoveCo()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(.2f);
        // Check to see if this Shape or another is matched
        if (otherShapeTile != null)
        {
            // if our current shape tile AND the other shape tile isn't matched - do the thing
            if (!isMatched && !otherShapeTile.GetComponent<ShapeTile>().isMatched)
            {
                // Set the other shape tiles position to this tiles position
                otherShapeTile.GetComponent<ShapeTile>().row = row;
                otherShapeTile.GetComponent<ShapeTile>().column = column;
                // Set this tiles row and tile position to its previous position
                row = previousRow;
                column = previousColumn;
            }
            else
            {
                board.DestroyMatches();
            }
            otherShapeTile = null;
        }
    }

    void FindMatches()
    {
        #region HORIZONTAL MATCHES

        // Search to find if this tile is apart of a match
        if (column > 0 && column < board.width - 1)
        {
            // Create a temp GameObject called leftTile1 and set it equal to the tile on the left of this one
            // only if this isnt in the column on the far left
            GameObject leftTile1 = board.allShapeTiles[column - 1, row];
            // Create a temp GameObject called rightTile1 and set it equal to the tile on the right of this one
            // only if this isnt the column on the far right
            GameObject rightTile1 = board.allShapeTiles[column + 1, row];
            // Check to see if the left and right tile's tag is equal to this gameObjects tag, if it is - do the thing

            if (leftTile1 != null && rightTile1 != null)
            {
                if (leftTile1.tag == this.gameObject.tag && rightTile1.tag == this.gameObject.tag)
                {
                    // This tile, as well as the left and right tiles are now matched
                    isMatched = true;
                    leftTile1.GetComponent<ShapeTile>().isMatched = true;
                    rightTile1.GetComponent<ShapeTile>().isMatched = true;
                }
            }
        }

        #endregion

        #region VERTICAL MATCHES

        // Search to find if this tile is apart of a match
        if (row > 0 && row < board.height - 1)
        {
            // Create a temp GameObject called leftTile1 and set it equal to the tile on the left of this one
            // only if this isnt in the column on the far left
            GameObject upTile1 = board.allShapeTiles[column, row + 1];
            // Create a temp GameObject called rightTile1 and set it equal to the tile on the right of this one
            // only if this isnt the column on the far right
            GameObject downTile1 = board.allShapeTiles[column, row - 1];
            // Check to see if the left and right tile's tag is equal to this gameObjects tag, if it is - do the thing

            if (upTile1 != null && downTile1 != null)
            {
                if (upTile1.tag == this.gameObject.tag && downTile1.tag == this.gameObject.tag)
                {
                    // This tile, as well as the left and right tiles are now matched
                    isMatched = true;
                    upTile1.GetComponent<ShapeTile>().isMatched = true;
                    downTile1.GetComponent<ShapeTile>().isMatched = true;
                }
            }
        }
        #endregion
    }
}
