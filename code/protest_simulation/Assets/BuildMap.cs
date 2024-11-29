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
        Color targetColor = new Color(0.92f, 0.92f, 0.92f); // Close to #EBEBEB
        float tolerance = 0.05f; // Adjust tolerance for color detection

        for (int x = 0; x < mapTexture.width; x++)
        {
            for (int y = 0; y < mapTexture.height; y++)
            {
                Color pixelColor = mapTexture.GetPixel(x, y);
                if (IsColorMatch(pixelColor, targetColor, tolerance))
                {
                    Vector2 position = new Vector2(x, y);


                    

                    Instantiate(buildingPrefab, position, Quaternion.identity);
                    
                }
            }
        }

        //TODO check if this can be done elsewhere (I think it executes on every update!!! and also doesn't quite work yet)
        //buildingPrefab.layer = LayerMask.NameToLayer("Obstacle");
        //if (buildingPrefab.GetComponent<BoxCollider2D>() == null) {
        //    boxCollider = buildingPrefab.AddComponent<BoxCollider2D>();
        //}



    }

    bool IsColorMatch(Color color, Color target, float tolerance)
    {
        return Mathf.Abs(color.r - target.r) < tolerance &&
               Mathf.Abs(color.g - target.g) < tolerance &&
               Mathf.Abs(color.b - target.b) < tolerance;
    }
}
