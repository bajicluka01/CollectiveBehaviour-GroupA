
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Null")]
public class NullBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {
         return Vector2.zero;
    }
}