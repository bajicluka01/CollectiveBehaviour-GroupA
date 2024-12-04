using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BystanderBehavior : ScriptableObject {
    public abstract Vector2 CalculateMove (BystanderAgent agent, List<Transform> context, BystanderFlock flock); 

    
}
