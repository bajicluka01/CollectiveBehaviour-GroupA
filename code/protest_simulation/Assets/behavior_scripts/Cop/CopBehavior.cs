using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CopBehavior : ScriptableObject {
    public abstract Vector2 CalculateMove (CopAgent agent, List<Transform> context, CopFlock flock); 

    
}
