using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteCutter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector2[] originalUVs; // Store original texture coordinates

    void Start()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Store the original texture coordinates
        originalUVs = spriteRenderer.sprite.vertices;
    }

    // Method to cut off the bottom portion of the sprite
    public void CutBottomPixels(int pixelsToCut)
    {
        Vector2[] newUVs = new Vector2[4];

        // Calculate the new texture coordinates
        float yOffset = pixelsToCut / (float)spriteRenderer.sprite.texture.height;
        newUVs[0] = new Vector2(0f, 0f);
        newUVs[1] = new Vector2(0f, 1f - yOffset);
        newUVs[2] = new Vector2(1f, 1f - yOffset);
        newUVs[3] = new Vector2(1f, 0f);

        // Apply the new texture coordinates to the sprite
        spriteRenderer.sprite.OverrideGeometry(newUVs, spriteRenderer.sprite.triangles);
    }

    // Method to reset the sprite to its original state
    public void ResetSprite()
    {
        // Restore the original texture coordinates
        spriteRenderer.sprite.OverrideGeometry(originalUVs, spriteRenderer.sprite.triangles);
    }
}