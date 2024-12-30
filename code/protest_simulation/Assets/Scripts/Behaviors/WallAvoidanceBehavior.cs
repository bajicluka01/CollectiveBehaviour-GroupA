using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Flock/Behavior/Wall Avoidance")]
public class WallAvoidanceBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {
        
        return Vector2.zero;
    }
}
