using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    wait,
    move,
    win,
    lose,
    pause
}

public enum TileKind
{
    Breakable,
    Blank,
    Locked,
    Concrete,
    Slime,
    Normal
}

[System.Serializable]
public class TileType
{
    public int x; // X value in grid
    public int y; // Y value in grid
    public TileKind tileKind;
}
[System.Serializable]
public class MatchType
{
    public int type;
    public string colour;
}

public class Board : MonoBehaviour
{
    [Header("Scriptable Objects")]
    public World world;
    public int level;

    [Header("Board Variables")]
    public GameState currentState = GameState.move; // Setting the current state value (for state machine)
    public int width; // The width of the board
    public int height; // The height of the board
    public int offSet; // The offset which the tiles are being created
    [Space(5)]
    [Header("Shape Tiles")]
    public GameObject[] shapeTiles; // The shape tiles to be instantiated
    [Header("Obstacle Tiles")]
    public GameObject backgroundTilePrefab; // The Background tile to be instantiated
    public GameObject breakableTilePrefab; // The breakable tile to be instantiated
    public GameObject lockedTilePrefab; // The locked tile to be instantiated
    public GameObject concreteTilePrefab; // The concrete tile to be instantiated
    public GameObject slimeTilePrefab; // The slime tile to be instantiated
    [Header("Board Layout Array")]
    public TileType[] boardLayout;
    [Space(5)]
    [Header("Destroy Effect")]
    public GameObject destroyEffect;
    [Space(5)]
    [Header("Obstacle Tile Arrays")]
    private bool[,] blankSpaces;  // An array of all of the background tiles
    public BackgroundTile[,] breakableTiles; // An array all of the breakable tiles
    public BackgroundTile[,] lockedTiles; // An array of the locked tiles
    public BackgroundTile[,] concreteTiles; // An array of the concrete tiles
    public BackgroundTile[,] slimeTiles; // An array of the slime tiles

    public GameObject[,] allShapeTiles; // An array of all of the shape tiles
    public ShapeTile currentTile;

    private FindMatches findMatches;
    private ScoreManager scoreManager;

    [Space(5)]
    [Header("Match Type")]
    public MatchType matchType;
    private bool makeSlime = true;
    [Space(5)]
    [Header("Score")]
    public int basePieceValue = 20;
    private int streakValue = 1;
    public int[] scoreGoals;
    private GoalManager goalManager;
    [Space(5)]
    [Header("Time Delays")]
    public float refillDelay = 0.5f;
    [Space(5)]
    private SoundManager soundManager;


    private void Awake()
    {
        // reset the level to be whatever is in player prefs
        if (PlayerPrefs.HasKey("Current Level"))
        {
            level = PlayerPrefs.GetInt("Current Level");
        }

        // If the world reference is not null
        if (world != null)
        {
            if (level < world.levels.Length)
            {
                // If the level reference we're trying to access is not null
                if (world.levels[level] != null)
                {
                    // set the width and the height of the board
                    width = world.levels[level].width;
                    height = world.levels[level].height;
                    // set which tiles will be found in the level
                    shapeTiles = world.levels[level].shapeTiles;
                    scoreGoals = world.levels[level].scoreGoals;
                    // set the board layout (are there any blank spaces or breakables?)
                    boardLayout = world.levels[level].boardLayout;

                }
            }
        }
    }

    void Start()
    {
        // Ensure that the game starts out being paused
        currentState = GameState.pause;

        goalManager = FindObjectOfType<GoalManager>(); // Initialise the GoalManager component
        findMatches = FindObjectOfType<FindMatches>(); // Initialise the FindMatches component
        soundManager = FindObjectOfType<SoundManager>(); // Initialise the Sound Manager
        scoreManager = FindObjectOfType<ScoreManager>(); // Initialise the Score Manager
        
        blankSpaces = new bool [width, height]; // Initialise the blankSpaces array
        breakableTiles = new BackgroundTile[width, height]; // Initialise the breakableTiles array
        lockedTiles = new BackgroundTile[width, height]; // Initialise the lockedTiles array
        concreteTiles = new BackgroundTile[width, height]; // Initialise the concreteTiles array
        slimeTiles = new BackgroundTile[width, height]; // Initialise the slimeTiles array

        allShapeTiles = new GameObject[width, height]; // Initialise the allShapeTiles array

        SetUp(); // Call the SetUp function
    }

    // Generate all the blank spaces on the board
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

    // Generate breakable tiles
    public void GenerateBreakableTiles()
    {
        // Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // If a tile is a breakable tile
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                // Create a temporary position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                // create a breakable tile at that position
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    // Generate Locked Tiles
    public void GenerateLockedTiles()
    {
        // Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // If a tile is a locked tile
            if (boardLayout[i].tileKind == TileKind.Locked)
            {
                // Create a temporary position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                // create a locked tile at that position
                GameObject tile = Instantiate(lockedTilePrefab, tempPosition, Quaternion.identity);
                lockedTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    // Generate Concrete Tiles
    public void GenerateConcreteTiles()
    {
        // Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // If a tile is a locked tile
            if (boardLayout[i].tileKind == TileKind.Concrete)
            {
                // Create a temporary position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                // create a locked tile at that position
                GameObject tile = Instantiate(concreteTilePrefab, tempPosition, Quaternion.identity);
                concreteTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    // Generate the Slime Tiles
    public void GenerateSlimeTiles()
    {
        // Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // If a tile is a slime tile
            if (boardLayout[i].tileKind == TileKind.Slime)
            {
                // Create a temporary position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                // create a slime tile at that position
                GameObject tile = Instantiate(slimeTilePrefab, tempPosition, Quaternion.identity);
                slimeTiles[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    // SetUp the board
    private void SetUp()
    {
        GenerateBlankSpaces(); // Generate the blank spaces
        GenerateBreakableTiles(); // Generate the breakable tiles
        GenerateLockedTiles(); // Generate the locked tiles
        GenerateConcreteTiles(); // Generate the concrete tiles
        GenerateSlimeTiles(); // Generate the slime tiles
        // Cycle through the Width and Height and Instantiate a Tile Prefab at
        // every position
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j] && !concreteTiles[i, j] && !slimeTiles[i, j])
                {
                    // Create a new temporary variable - a Vector2 for the W & H pos
                    Vector2 tempPosition = new Vector2(i, j + offSet);

                    Vector2 tilePosition = new Vector2(i, j);

                    // Instantiate the backgroundTilePrefab as a GameObject
                    GameObject backgroundTile = Instantiate(backgroundTilePrefab, tilePosition, Quaternion.identity) as GameObject;
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
    private MatchType ColumnOrRow()
    {
        // make a copy of the current matches
        List<GameObject> matchCopy = findMatches.currentMatches as List<GameObject>;

        matchType.type = 0;
        matchType.colour = "";

        // cycle through all of matchCopy and decide if a bombs needs to be made
        for (int i = 0; i < matchCopy.Count; i++)
        {
            // store the spaeTile[i]
            ShapeTile thisTile = matchCopy[i].GetComponent<ShapeTile>();

            // Set a tenp variable colour equal to the matchCopy[i]'s tag
            string colour = matchCopy[i].tag;

            // store the column and row
            int column = thisTile.column;
            int row = thisTile.row;
            int columnMatch = 0;
            int rowMatch = 0;
            // cycle through the rest of the pieces and compare
            for (int j = 0; j < matchCopy.Count; j++)
            {
                // store the next tile
                ShapeTile nextTile = matchCopy[j].GetComponent<ShapeTile>();
                // is the next tile the same as this tile
                if (nextTile == thisTile)
                {
                    continue;
                }
                // check to see if it's a column match
                if (nextTile.column == thisTile.column && nextTile.tag == colour)
                {
                    columnMatch++;
                }
                // check to see if it's a row match
                if (nextTile.row == thisTile.row && nextTile.tag == colour)
                {
                    rowMatch++;
                }
            }
            // return 1 if its colour bomb
            if (columnMatch == 4 || rowMatch == 4)
            {
                matchType.type = 1;
                matchType.colour = colour;
                return matchType;
            // return 2 if its adjacent
            }
            else if (columnMatch == 2 && rowMatch == 2)
            {
                matchType.type = 2;
                matchType.colour = colour;
                return matchType;
            // return 3 if its column or row
            }
            else if (columnMatch == 3 || rowMatch == 3)
            {
                matchType.type = 3;
                matchType.colour = colour;
                return matchType;
            }
        }

        matchType.type = 0;
        matchType.colour = "";
        return matchType;
        #region OLD CODE
        /*
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
        */
        #endregion
    }

    private void CheckToMakeBombs()
    {
        // how many objects are in findMatches/ currenmatcher
        if (findMatches.currentMatches.Count > 3)
        {
            // what type of match has been made
            MatchType typeOfMatch = ColumnOrRow();
            if (typeOfMatch.type == 1)
            {
                // Make a colour bomb
                // Is the current tile matched?
                // Unmatch it and turn it into a colour bomb
                if (currentTile != null && currentTile.isMatched && currentTile.tag == typeOfMatch.colour)
                {
                    
                        currentTile.isMatched = false;
                        currentTile.MakeColourBomb();
                }
                else
                {
                    if (currentTile.otherShapeTile != null)
                    {
                        ShapeTile otherTile = currentTile.otherShapeTile.GetComponent<ShapeTile>();
                        if (otherTile.isMatched && otherTile.tag == typeOfMatch.colour)
                        {
                                otherTile.isMatched = false;
                                otherTile.MakeColourBomb();
                        }
                    }
                }
            }
        
            else if (typeOfMatch.type == 2)
            {
                // Make an adjacent bomb
                // Is the current tile matched?
                // Unmatch it and turn it into a colour bomb
                if (currentTile != null && currentTile.isMatched && currentTile.tag == typeOfMatch.colour)
                {
                    currentTile.isMatched = false;
                    currentTile.MakeAdjacentBomb();
                        
                }
                else
                {
                    if (currentTile.otherShapeTile != null)
                    {
                        ShapeTile otherTile = currentTile.otherShapeTile.GetComponent<ShapeTile>();
                        if (otherTile.isMatched && otherTile.tag == typeOfMatch.colour)
                        {
                            otherTile.isMatched = false;
                            otherTile.MakeAdjacentBomb();
                        }
                    }
                }
            }
            else if (typeOfMatch.type == 3)
            {
                findMatches.CheckBombs(matchType);
            }

            #region OLD CODE
            /*
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
            */
            #endregion
        }
    }

    // This function handles destroying matches at the correct column and row on the board
    private void DestroyMatchesAt(int column, int row)
    {
        // Destroy matches at position
        // Check if shape is matched, destroy
        if (allShapeTiles[column, row].GetComponent<ShapeTile>().isMatched)
        {

            // Does a tile need to break?
            if (breakableTiles[column, row] != null)
            {
                // if it does, give one damage
                breakableTiles[column, row].TakeDamage(1); // DONT HARD CODE THIS VALUE 
                if (breakableTiles[column, row].hitPoints <= 0)
                {
                    // remove from array
                    breakableTiles[column, row] = null;
                }
            }

            if (lockedTiles[column, row] != null)
            {
                // if it does, give one damage
                lockedTiles[column, row].TakeDamage(1); // DONT HARD CODE THIS VALUE 
                if (lockedTiles[column, row].hitPoints <= 0)
                {
                    // remove from array
                    lockedTiles[column, row] = null;
                }
            }

            DamageConcrete(column, row);
            DamageSlime(column, row);

            // Check for goals
            if (goalManager != null)
            {
                // Compare tag
                goalManager.CompareGoal(allShapeTiles[column, row].tag.ToString());
                // Update goal
                goalManager.UpdateGoals();
            }
            
            // Does the soundmanager exist
            if (soundManager != null)
            {
                soundManager.PlayRandomDestroyNoise();
            }
            else
            {
                return;
            }
            

            // Instantiate Particle effect
            GameObject particle = Instantiate(destroyEffect, allShapeTiles[column, row].transform.position, Quaternion.identity);
            Destroy(particle, 1f);

            // Destroy the correct tile at the column and row position
            Destroy(allShapeTiles[column, row]);
            //Increase score
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            // Set that coord to null (leaving it empty)
            allShapeTiles[column, row] = null;
        }
    }

    // This method destroys the tiles after they have been matched
    public void DestroyMatches()
    {
        // How many elements are in the matched pieces list from findmatches?
        if (findMatches.currentMatches.Count >= 4)
        {
            // call the CheckToMakeBombs method
            CheckToMakeBombs();
        }

        findMatches.currentMatches.Clear();

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
        
        //StartCoroutine(DecreaseRowCo());
        StartCoroutine(DecreaseRowCo2());
    }

    private void DamageConcrete(int column, int row)
    {
        // if the column we're checking is greater than 0
        if (column > 0)
        {
            // if the concrete tiles column is valued at column - 1
            if (concreteTiles[column - 1, row])
            {
                // deal X damage to the concrete tile
                concreteTiles[column - 1, row].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (concreteTiles[column - 1, row].hitPoints <= 0)
                {
                    concreteTiles[column - 1, row] = null;
                }
            }
        }
        if (column < width - 1)
        {
            // if the concrete tiles column is valued at column - 1
            if (concreteTiles[column + 1, row])
            {
                // deal X damage to the concrete tile
                concreteTiles[column + 1, row].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (concreteTiles[column + 1, row].hitPoints <= 0)
                {
                    concreteTiles[column + 1, row] = null;
                }
            }
        }
        if (row > 0)
        {
            // if the concrete tiles column is valued at column - 1
            if (concreteTiles[column, row - 1])
            {
                // deal X damage to the concrete tile
                concreteTiles[column, row - 1].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (concreteTiles[column, row - 1].hitPoints <= 0)
                {
                    concreteTiles[column, row - 1] = null;
                }
            }
        }
        if (row < height - 1)
        {
            // if the concrete tiles column is valued at column - 1
            if (concreteTiles[column, row + 1])
            {
                // deal X damage to the concrete tile
                concreteTiles[column, row + 1].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (concreteTiles[column, row + 1].hitPoints <= 0)
                {
                    concreteTiles[column, row + 1] = null;
                }
            }
        }
    }

    private void DamageSlime(int column, int row)
    {
        // if the column we're checking is greater than 0
        if (column > 0)
        {
            // if the concrete tiles column is valued at column - 1
            if (slimeTiles[column - 1, row])
            {
                // deal X damage to the concrete tile
                slimeTiles[column - 1, row].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (slimeTiles[column - 1, row].hitPoints <= 0)
                {
                    slimeTiles[column - 1, row] = null;
                }
                makeSlime = false;
            }
        }
        if (column < width - 1)
        {
            // if the concrete tiles column is valued at column - 1
            if (slimeTiles[column + 1, row])
            {
                // deal X damage to the concrete tile
                slimeTiles[column + 1, row].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (slimeTiles[column + 1, row].hitPoints <= 0)
                {
                    slimeTiles[column + 1, row] = null;
                }
                makeSlime = false;
            }
        }
        if (row > 0)
        {
            // if the concrete tiles column is valued at column - 1
            if (slimeTiles[column, row - 1])
            {
                // deal X damage to the concrete tile
                slimeTiles[column, row - 1].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (slimeTiles[column, row - 1].hitPoints <= 0)
                {
                    slimeTiles[column, row - 1] = null;
                }
                makeSlime = false;
            }
        }
        if (row < height - 1)
        {
            // if the concrete tiles column is valued at column - 1
            if (slimeTiles[column, row + 1])
            {
                // deal X damage to the concrete tile
                slimeTiles[column, row + 1].TakeDamage(1);
                // if the concrete tile's hp is less than or equal to 0, set it to null
                if (slimeTiles[column, row + 1].hitPoints <= 0)
                {
                    slimeTiles[column, row + 1] = null;
                }
                makeSlime = false;
            }
        }
    }

    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // If the current spot isn't blank and is empty
                if (!blankSpaces[i, j] && allShapeTiles[i, j] == null && !concreteTiles[i, j] && !slimeTiles[i, j])
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
        yield return new WaitForSeconds(refillDelay * 0.5f);
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

        yield return new WaitForSeconds(refillDelay * 0.5f);
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
                if (allShapeTiles[i, j] == null && !blankSpaces[i, j] && !concreteTiles[i, j] && !slimeTiles[i,j] )
                {
                    // Instantiate a new tile in it's place
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int tileToUse = Random.Range(0, shapeTiles.Length);

                    // Make sure we arent placing a piece into a spot where there isnt a match
                    int maxIterations = 0;
                    while (MatchesAt(i, j, shapeTiles[tileToUse]) && maxIterations < 100)
                    {
                        // if there is a match, increase iterations
                        maxIterations++;
                        // re-assign the tileToUse
                        tileToUse = Random.Range(0, shapeTiles.Length);
                    }
                    // Reset iterations
                    maxIterations = 0;

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
        findMatches.FindAllMatches();
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
        // Wait for half a second
        yield return new WaitForSeconds(refillDelay);
        // Call the Refill Board function
        RefillBoard();

        // While MatchesOnBoard returns true
        while (MatchesOnBoard())
        {
            // Increase the streak value
            streakValue ++;
            // Then call the DestroyMatches function
            DestroyMatches();
            // Wait for half a second
            yield break;
            //yield return new WaitForSeconds(2 * refillDelay);
        }
        // Clear the current matches list to prevent any mistaken match 7 column/ row bombs
        //findMatches.currentMatches.Clear();
        // Wait for half a second
        yield return new WaitForSeconds(refillDelay);

        // check to make slime
        CheckToMakeSlime();

        // Finding deadlock
        if (IsDeadlocked())
        {
            ShuffleBoard();
            Debug.Log("DEADLOCKED!");
        }

        if (currentState != GameState.pause)
        {
            // Set the current game state to move
            currentState = GameState.move;
        }
        
        makeSlime = true;
        // Reset the streak value
        streakValue = 1;
    }

    // Helper method to switch pieces
    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        // Take the second piece and save it in a holder
        GameObject holder = allShapeTiles[column + (int)direction.x, row + (int)direction.y] as GameObject;
        // Switching the first tile to be the second pos
        allShapeTiles[column + (int)direction.x, row + (int)direction.y] = allShapeTiles[column, row];
        // Set the first tile to be the second tile
        allShapeTiles[column, row] = holder;
    }

    // Helper method
    private void CheckToMakeSlime()
    {
        // check the slime tiles array
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (slimeTiles[i, j] != null && makeSlime)
                {
                    // Call method to make new slime
                    MakeNewSlime();
                }
            }
        }
    }

    // Check for adjacent slime pieces
    private Vector2 CheckForAdjacent(int column, int row)
    {
        // check right
        if (allShapeTiles[column + 1, row] && column < width - 1)
        {
            // there IS a piece on the right
            return Vector2.right;
        }
        // check left
        if (allShapeTiles[column - 1, row] && column > 0)
        {
            // there IS a piece on the left
            return Vector2.left;
        }
        // check above
        if (allShapeTiles[column, row + 1] && row < height - 1)
        {
            // there IS a piece above
            return Vector2.up;
        }
        // if we have a tile to the right and the column is less than width minus 1
        if (allShapeTiles[column, row - 1] && row > 0)
        {
            // there IS a piece below
            return Vector2.down;
        }
        return Vector2.zero;
    }

    // Make a new slime if the current one hasnt been destroyed
    private void MakeNewSlime()
    {
        bool slime = false;
        int loops = 0;
        while (!slime && loops < 200)
        {
            int newX = Random.Range(0, width);
            int newY = Random.Range(0, height);

            if (slimeTiles[newX, newY])
            {
                // Check to see if there is an adjacent and return a dir
                Vector2 adjacent = CheckForAdjacent(newX, newY);
                if (adjacent != Vector2.zero)
                {
                    Destroy(allShapeTiles[newX + (int)adjacent.x, newY + (int)adjacent.y]);
                    Vector2 tempPos = new Vector2(newX + (int)adjacent.x, newY + (int)adjacent.y);
                    GameObject tile = Instantiate(slimeTilePrefab, tempPos, Quaternion.identity);
                    slimeTiles[newX + (int)adjacent.x, newY + (int)adjacent.y] = tile.GetComponent<BackgroundTile>();
                    slime = true;
                }
            }

            loops++;
            currentState = GameState.move;
        }
    }

    // Helper method capable of checking for matches -> return true or false
    private bool CheckForMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Make sure its not an empty space
                if (allShapeTiles[i, j] != null)
                {
                    // Make sure going to the right are in the board
                    if (i < width - 2)
                    {
                        // Check if the tiles to the right and two to the right exist
                        if (allShapeTiles[i + 1, j] != null && allShapeTiles[i + 2, j] != null)
                        {
                            // Check if the tags are the same -> if there are, there is a match.
                            if (allShapeTiles[i + 1, j].tag == allShapeTiles[i, j].tag
                               && allShapeTiles[i + 2, j].tag == allShapeTiles[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }

                    if (j < height - 2)
                    {
                        // Check if the tiles above exist
                        if (allShapeTiles[i, j + 1] != null && allShapeTiles[i, j + 2] != null)
                        {
                            // Check if the tags are the same -> if there are, there is a match.
                            if (allShapeTiles[i, j + 1].tag == allShapeTiles[i, j].tag
                               && allShapeTiles[i, j + 2].tag == allShapeTiles[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }

    private bool IsDeadlocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allShapeTiles[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            // There is a match
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            // There is a match
                            return false;
                        }
                    }
                }
            }
        }
        // If DEADLOCKED -> return true
        return true;
    }

    // This method will shuffle the board if it becomes deadlocked (recursive method)
    private void ShuffleBoard()
    {
        // Create a list of gameobjects
        List<GameObject> newBoard = new List<GameObject>();
        // Add every piece to list
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allShapeTiles[i, j] != null)
                {
                    newBoard.Add(allShapeTiles[i, j]);
                }
            }
        }
        // for every spot on the board
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // If this spot shouldnt be blank
                if (!blankSpaces[i, j] && !concreteTiles[i, j] && !slimeTiles[i, j])
                {
                    // Pick a rand number
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    
                    // make sure the while loop doesnt become infinite
                    int maxIterations = 0;

                    // Check to see if there are matches, if there are, choose a different shape tile
                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                    {
                        // Choose a new tile to instantiate
                        pieceToUse = Random.Range(0, newBoard.Count);
                        // Increase the maxIterations by 1
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    // Piece container
                    ShapeTile piece = newBoard[pieceToUse].GetComponent<ShapeTile>();
                    // Reset maxIterations
                    maxIterations = 0;
                    // Set the piece column to i
                    piece.column = i;
                    // Set the piece row to j
                    piece.row = j;
                    // Fill in the tiles array with new piece
                    allShapeTiles[i, j] = newBoard[pieceToUse];
                    // Remove from list
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }
        // check if the board is still deadlocked
        if (IsDeadlocked())
        {
            ShuffleBoard();
        }
    }
}
