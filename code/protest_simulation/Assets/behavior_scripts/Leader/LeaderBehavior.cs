using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeaderBehavior : ScriptableObject {
    public abstract Vector2 CalculateMove (LeaderAgent agent, List<Transform> context, LeaderFlock flock); 

    
}
