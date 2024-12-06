using System.Collections.Generic;
using UnityEngine;

// this class describes how protestors react to the leader that is present in the simulation
[CreateAssetMenu(menuName = "Protester/Behavior/Protester follow leader")]
public class FollowLeaderBehavior : FlockBehavior 
{
    
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        FlockAgent leaderAgent = flock.Leader;
        return GetLeaderDirectionVector(agent,leaderAgent);
    }

    // this returns the
    Vector2 GetLeaderDirectionVector(FlockAgent protester, FlockAgent leaderAgent) {
        Vector2 directionVector = (-(protester.transform.position) + leaderAgent.transform.position);
        return directionVector;
    }
}

