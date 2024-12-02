using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BuildMap : MonoBehaviour
{
    public Texture2D mapTexture; // Assign this in the Inspector
    public GameObject buildingPrefab; // Assign a prefab for the buildings

    public BoxCollider2D boxCollider;

    void Start()
    {

        CreateBuildingsFromMap();
    }

    void CreateBuildingsFromMap()
    {
        Color targetColor = new Color(1.0f, 1.0f, 1.0f); 

        //TODO:
        //this tolerance shouldn't have any impact on the map (because it's generated from a binary image), but for some reason it does. Look into it!!! 
        float tolerance = 0.1f; 

        //this controls the placement of the map (i.e. to ensure that the agents start in a central location)
        float initialX = -100f;
        float initialY = -100f;

        for (int x = 0; x < mapTexture.width; x++)
        {
            for (int y = 0; y < mapTexture.height; y++)

            {
                Color pixelColor = mapTexture.GetPixel(x, y);
                if (IsColorMatch(pixelColor, targetColor, tolerance))
                {
                    Vector2 position = new Vector2(initialX+x, initialY+y);
                    GameObject obj = Instantiate(buildingPrefab, position, Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform, false);
                }
            }
        }
    }

    bool IsColorMatch(Color color, Color target, float tolerance)
    {
        return Mathf.Abs(color.r - target.r) < tolerance &&
               Mathf.Abs(color.g - target.g) < tolerance &&
               Mathf.Abs(color.b - target.b) < tolerance;
    }
}
