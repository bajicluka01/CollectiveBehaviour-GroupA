using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Flock/Behavior/End position seeking")]
public class EndPositionSeekingBehavior : FlockBehavior
{
    float maxDistanceOfDesiredPosition = 30f;
    float minDistanceOfDesiredPosition = 15f;
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {
        if (agent.DesiredPosition == Vector3.zero)
        {
            agent.DesiredPosition = GenerateNewDesiredPosition(minDistanceOfDesiredPosition, maxDistanceOfDesiredPosition, agent.transform.position);
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

    Vector2 GenerateNewDesiredPosition(float minDistance, float maxDistance, Vector3 agentPosition)
    {
        // Generate a random angle
        float angle = Random.Range(0f, 2f * Mathf.PI);
        // Generate a random distance from 0 to maxDistance
        float randomDistance = Random.Range(minDistance, maxDistance);
        
        // Calculate the random position using polar coordinates
        float x = agentPosition.x + randomDistance * Mathf.Cos(angle);
        float y = agentPosition.y + randomDistance * Mathf.Sin(angle);
        
        // TODO: implement a check if this position is not in a building 
        // - if it is inside a building recalculate
        // else return the result
        
        return new Vector2(x, y);
    }
}