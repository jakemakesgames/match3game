using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public int hitPoints; // how many swipes it takes in order to break
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (hitPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        MakeLighter();
    }

    // This makes the breakable tile lighter -> could do a sprite swap later/ once new art is in
    void MakeLighter()
    {
        Color colour = spriteRenderer.color; // Take current colour
        float newAlpha = colour.a * .5f; // Get current colours alpha
        spriteRenderer.color = new Color(colour.r,colour.g, colour.b, newAlpha); // Set the colour to a new colour (50 lighter)

    }
}
