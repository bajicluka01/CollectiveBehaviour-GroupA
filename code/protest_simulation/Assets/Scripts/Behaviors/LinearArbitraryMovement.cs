using System.Collections.Generic;
using UnityEngine;


//this is just for testing purposes (movement in a single direction, unless you encounter wall, etc.)
[CreateAssetMenu(menuName = "Flock/Behavior/Linear Arbitrary Movement")]
public class LinearArbitraryMovement : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {
         return Vector2.right;
    }
}
