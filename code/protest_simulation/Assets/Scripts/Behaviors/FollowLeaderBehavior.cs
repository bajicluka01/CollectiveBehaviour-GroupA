using System.Collections.Generic;
using UnityEngine;

// this class describes how protestors react to the leader that is present in the simulation
[CreateAssetMenu(menuName = "Flock/Behavior/Protester follow leader")]
public class FollowLeaderBehavior : FlockBehavior 
{
    
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock) 
    {
        Vector2 result = new();
        return result;
    }

    // this returns the
    Vector2 GetLeaderDirectionVector(FlockAgent protester, FlockAgent leaderAgent) 
    {
        Vector2 directionVector = leaderAgent.transform.position - protester.transform.position ;
        directionVector.Normalize();

        Debug.Log(directionVector);
        return directionVector;
    }
}

