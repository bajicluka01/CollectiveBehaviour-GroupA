using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Bystander Leader Follow Behavior")]
public class BystanderLeaderFollowBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
         return Vector2.zero;
    }
}