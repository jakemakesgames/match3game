using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level : ScriptableObject
{
    [Header("Board Dimensions")]
    public int width; // width of board
    public int height; // height of board

    [Header("Starting Tiles")]
    public TileType[] boardLayout; // an array of the board's starting layout

    [Header("Available Tiles")]
    public GameObject[] shapeTiles; // an array of all of the shapeTiles available in the level

    [Header("Score Goals")]
    public int[] scoreGoals; // the score goals for the level

    [Header("End Game Requirements")]
    public EndGameRequirements endGameRequirements; // the end game requirements for the level
    public BlankGoal[] levelGoals; // the goals required

}
