using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// NOTE: this class now implements the follow leader behaviour since it sets the desired position of 
// the agent to the leader position
[CreateAssetMenu(menuName = "Flock/Behavior/End position seeking")]
public class EndPositionSeekingBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        // if (GroupContext.LeaderPositionInContext(context))
        // {
        //     Vector3 leaderPosition = GroupContext.GetLeaderPosition(context);
        //     Vector2 leaderDirection = leaderPosition - agent.transform.position;
        //     leaderDirection.Normalize();
        //     return agent.DesiredSpeed*leaderDirection - agent.PreviousMove;
        // } else
        if (agent.DesiredPosition == Vector3.zero )
        {
            agent.DesiredPosition = GenerateNewDesiredPosition(agent);
        }
        if (agent.OnDesiredPosition())
        {
            agent.State = AgentState.Stationary;
            agent.DesiredPosition = Vector3.zero;
            return Vector2.zero;
        }
        Vector2 desiredPositionVector = agent.DesiredPosition - agent.transform.position;
        desiredPositionVector.Normalize();

        return agent.DesiredSpeed*desiredPositionVector - agent.PreviousMove;
    }

    // IDEA: here we can take the center (same calculation and everything)
    // of the flock protestors. So take in the entire flock 
    //
    //    (agents.Count(agent => agent.Role == AgentRole.Protester));
    //    (agents.Count(agent => agent.Role == AgentRole.Bystander));
    //    (agents.Count(agent => agent.Role == AgentRole.Police));
    //
    Vector2 GenerateNewDesiredPosition(FlockAgent agent)
    {
        agent.LookAround(150);
        List<Vector3> agentPositions = agent.visibleProtesters.Select(protester => protester.transform.position).ToList(); 
        if (agentPositions.Count() < 1)
        {
            float minDistance = Random.Range(5,10);
            float maxDistance = Random.Range(minDistance,20);
            return GenerateRandomPositionInsideRing(minDistance, maxDistance, agent.transform.position);
        }

        Vector3 center = agentPositions.Aggregate(Vector3.zero, (curr, vec) => curr + vec) / agentPositions.Count();
        float distance = agentPositions.OrderByDescending(v => Vector3.Distance(v, center)).First().magnitude;
        if (agent.Role == AgentRole.Protester)
        {
            return GenerateRandomPositionInsideRing(0f, distance, center);
        }
        else
        {
            // distance/2 is a random made up constanct by the one and only GENIUS NIK
            return GenerateRandomPositionInsideRing(distance, distance+(distance/2), center);
        }
    }

    Vector2 GenerateRandomPositionInsideRing(float minDistance, float maxDistance, Vector3 center)
    {
        // Generate a random angle
        float angle = Random.Range(0f, 2f * Mathf.PI);
        // Generate a random distance from 0 to maxDistance
        float randomDistance = Random.Range(minDistance, maxDistance);
        
        // Calculate the random position using polar coordinates
        float x = center.x + randomDistance * Mathf.Cos(angle);
        float y = center.y + randomDistance * Mathf.Sin(angle);
        
        // TODO: implement a check if this position is not in a building !!!!!!!  
        // - if it is inside a building recalculate
        // else return the result
        
        return new Vector2(x, y);
    }
}