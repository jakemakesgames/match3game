using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Data Container
[System.Serializable]
public class BlankGoal
{
    public int numberNeeded;
    public int numberCollected;
    public Sprite goalSprite;
    public string matchValue;
}

public class GoalManager : MonoBehaviour
{
    public BlankGoal[] levelGoals;

    public List<GoalPanel> currentGoals = new List<GoalPanel>();

    public GameObject goalPrefab;
    public GameObject goalIntroParent;
    public GameObject goalGameParent;

    // Start is called before the first frame update
    void Start()
    {
        SetupGoals();
    }

    void SetupGoals()
    {
        // Loop through all of the level goals in the array
        for (int i = 0; i < levelGoals.Length; i++)
        {
            // Create a new Goal panel at the goal intro parent position
            GameObject goal = Instantiate(goalPrefab, goalIntroParent.transform.position, Quaternion.identity);
            goal.transform.SetParent(goalIntroParent.transform);
            // Set the image and text of the goal:
            GoalPanel panel = goal.GetComponent<GoalPanel>();
            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/ " + levelGoals[i].numberNeeded;

            // Create the in game goals
            GameObject gameGoal = Instantiate(goalPrefab, goalGameParent.transform.position, Quaternion.identity);
            gameGoal.transform.SetParent(goalGameParent.transform);
            panel = gameGoal.GetComponent<GoalPanel>();

            currentGoals.Add(panel);

            panel.thisSprite = levelGoals[i].goalSprite;
            panel.thisString = "0/ " + levelGoals[i].numberNeeded;
        }
    }

    // This function updates the goal text as well as keeps track of whether the goals have been completed
    public void UpdateGoals()
    {
        int goalsCompleted = 0;
        // Loop through all of the goals
        for (int i = 0; i < levelGoals.Length; i++)
        {
            currentGoals[i].thisText.text = "" + levelGoals[i].numberCollected + "/" + levelGoals[i].numberNeeded;

            if (levelGoals[i].numberCollected >= levelGoals[i].numberNeeded)
            {
                goalsCompleted++;
                currentGoals[i].thisText.text = "" + levelGoals[i].numberNeeded + "/" + levelGoals[i].numberNeeded;
            }
        }
        if (goalsCompleted >= levelGoals.Length)
        {
            Debug.Log("Level Completed!");
        }
    }

    // Compare whether or not the tile being destroyed corresponds with a goal requirement -> if it does, update goal text
    public void CompareGoal(string goalToCompare)
    {
        // Loop through all goals
        for (int i = 0; i < levelGoals.Length; i++)
        {
            // Check to see if the goalToCompare is equal to matchValue
            if (goalToCompare == levelGoals[i].matchValue)
            {
                levelGoals[i].numberCollected++;
            }
        }
    }
}
