using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Flock/Behavior/Leader Manual Control")]
public class LeaderManualControlMovementBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        if (agent.manualMovement)
        {
            Vector2 result = Vector2.zero;
            if (Input.GetKey(KeyCode.W))
            {
                result += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                result += Vector2.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                result += Vector2.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                result += Vector2.left;
            }
            result.Normalize();
            return result;
        }
        else
        {
            if (agent.DesiredPosition == Vector3.zero || agent.OnDesiredPosition())
                agent.DesiredPosition = agent.GenerateNewDesiredPosition();
            return EndPositionSeekingBehavior.CalculateDesiredPositionWithDesiredSpeedVector(agent);
        }
    }
}
