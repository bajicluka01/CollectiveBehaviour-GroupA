using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProtesterBehavior : ScriptableObject {
    public abstract Vector2 CalculateMove (ProtesterAgent agent, List<Transform> context, ProtesterFlock flock); 

    
}
