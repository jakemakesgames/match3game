using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeTile : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swipeAngle = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
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
    }
}
