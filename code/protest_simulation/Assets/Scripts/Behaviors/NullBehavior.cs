using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Null")]
public class NullBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
         return Vector2.zero;
    }
}