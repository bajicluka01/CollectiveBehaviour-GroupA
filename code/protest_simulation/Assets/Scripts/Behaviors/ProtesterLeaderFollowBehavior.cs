using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Protester Leader Follow Behavior")]
public class ProtesterLeaderFollowBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
         return Vector2.zero;
    }
}