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
        Vector2 desiredPositionVector = agent.DesiredPosition - agent.transform.position;
        desiredPositionVector.Normalize();
        return agent.DesiredSpeed*desiredPositionVector - agent.PreviousMove;
    }

    // TODO: Luka remove this comment
    // NIK: Don't worry i just moved the methods to the flock agent class
    // this is because the methods will be used to handle agent state
    // the implementation before this commit where i moved everything there was kina janky
    // this is because even a small move caused the agent to pick a nother random position and start moving
    // resulting in what looked like a mass migration
    // now we can simply apply more behaviours to even stationary behaviour
}