using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Flock/Behavior/Social Force Model based behavior")]
public class SocialForceModelBehavior : FlockBehavior
{

    public float distanceOfDesiredPosition = 10f;
    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if (agent.DesiredPosition == Vector3.zero || agent.OnDesiredPosition())
        {
            agent.DesiredPosition = GenerateNewDesiredPosition(distanceOfDesiredPosition, agent.transform.position);
        }
        Vector2 desiredPositionVector = agent.DesiredPosition - agent.transform.position;
        desiredPositionVector.Normalize();

        return agent.DesiredSpeed*desiredPositionVector - agent.PreviousMove;
    }

    Vector2 GenerateNewDesiredPosition(float distance, Vector3 agentPosition)
    {
        // Generate a random angle
        float angle = Random.Range(0f, 2f * Mathf.PI);
        // Generate a random distance from 0 to maxDistance
        float randomDistance = Random.Range(0f, distance);
        
        // Calculate the random position using polar coordinates
        float x = agentPosition.x + randomDistance * Mathf.Cos(angle);
        float y = agentPosition.y + randomDistance * Mathf.Sin(angle);
        
        return new Vector2(x, y);
    }
}