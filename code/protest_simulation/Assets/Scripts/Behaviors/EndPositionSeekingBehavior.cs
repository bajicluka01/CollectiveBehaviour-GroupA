using UnityEngine;

// This behavior is intended for the moving state
// do not use this behaviour in the stationary state
[CreateAssetMenu(menuName = "Flock/Behavior/End position seeking")]
public class EndPositionSeekingBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        // TODO: 
        // we need some logic to make the agent give up if it can't reach desired position
        // (either because a building or barricade is in the way)
        //
        // TODO: how to implement this?
        // A:Draw a ray with the help of the normalized position vector
        // ---- if the ray is hitting either the police or a building than we cannot reach that place
        // ---- and we should then generate a new desired position and go to that one
        // ---- maybe add a pause -> this can be come to potting hte desired position to the current position
        // ----- the above will make the agnet stop because he knows that something is blcoking his path

        // this if here is only to prevent using this class to be used in stationary
        if (agent.DesiredPosition == Vector3.zero)
        {
            return Vector2.zero;
        }
        return CalculateDesiredPositionWithDesiredSpeedVector(agent, flock);
    }

    public static Vector2 CalculateDesiredPositionWithDesiredSpeedVector(FlockAgent agent, Flock flock)
    {
        Vector2 desiredPositionVector = agent.DesiredPosition - agent.transform.position;
        desiredPositionVector.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(agent.transform.position, desiredPositionVector, flock.eyesightDistance/4);
        if (hit && (hit.collider.CompareTag("map") || hit.collider.CompareTag("Police")))
        {
            agent.DesiredPosition = agent.GenerateNewDesiredPosition();
        }
        return desiredPositionVector;
    }
}