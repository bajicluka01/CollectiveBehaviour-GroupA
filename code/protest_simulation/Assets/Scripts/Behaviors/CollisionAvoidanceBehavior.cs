using System.Collections.Generic;
using UnityEngine;

// this is calcualted using the formulas from the social crowd simulation paper
// Social Crowd Simulation: Improving Realism with Social Rules and Gaze Behavior
[CreateAssetMenu(menuName = "Flock/Behavior/Collision Avoidance")]
public class CollisionAvoidanceBehavior : FlockBehavior
{
    readonly float theta = 0.9748f;
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {
        List<FlockAgent> nonGroupMembers = GroupContext.GetNonGroupMembers(agent, context);
        FlockAgent lowestTTCAgent = GetLowestTTCAgent(agent, nonGroupMembers);
        if (lowestTTCAgent != agent)
        {
            Vector3 wij = lowestTTCAgent.transform.position - agent.transform.position;
            Vector3 wu = CalculateWu(wij, agent);
            Vector3 wc = Vector3.Cross(wu,wij);
            wc.Normalize();
            // TODO: check if this is good
            // for the R parameter that is defined as the maximum possible distance between the 
            // float S = 1;
            // float R = 1;
            // for now i will just leave this as it is and implement other behaviours first
            return wc;
        }
        return Vector2.zero;
    }

    public Vector3 CalculateWu(Vector3 wij, FlockAgent agent)
    {
            if (Vector2.Dot(wij, agent.PreviousMove) >= theta)
            {
                Debug.Log("test");
                return Vector3.up;
            }
            else
            {
                return Vector3.Cross(wij,agent.PreviousMove);
            }
    }

    // this method handles the inputs so that if the minimum ttc is negative one 
    // where -1 means that there is no collision the agent itself is returned else
    // the coliding agent that has the min ttc is returned
    public FlockAgent GetLowestTTCAgent(FlockAgent agent, List<FlockAgent> nonGroupMembers)
    {
        float minTTc = -1;
        FlockAgent colidingAgent = agent;
        foreach (FlockAgent nonMember in nonGroupMembers)
        {
            float ttc = CalculateTimeToCollision(agent.transform.position, agent.PreviousMove,
                                                nonMember.transform.position, nonMember.PreviousMove,
                                                agent.ColiderRadius/2, nonMember.ColiderRadius/2);
            if (ttc != (-1) && ((minTTc == (-1)) || (ttc < minTTc)))
            {
                minTTc = ttc;
                colidingAgent = nonMember;
            }
        }
        return colidingAgent;
    }

    // TODO: check if this method is correct
    public float CalculateTimeToCollision(Vector3 positionA, Vector2 velocityA,
                                        Vector3 positionB, Vector2 velocityB,
                                        float radiusA, float radiusB)
    {
        Vector3 relativeVelocity = velocityA - velocityB;
        Vector3 relativePosition = positionA - positionB;
        float combinedRadii = radiusA + radiusB;
        float a = Vector3.Dot(relativeVelocity, relativeVelocity);
        float b = 2 * Vector3.Dot(relativePosition, relativeVelocity);
        float c = Vector3.Dot(relativePosition, relativePosition) - combinedRadii * combinedRadii;
        float discriminant = b * b - 4 * a * c;
        if (discriminant < 0 || a == 0)
        {
            // No collision or objects are not moving
            return -1;
        }
        float sqrtDiscriminant = Mathf.Sqrt(discriminant);
        float t1 = (-b - sqrtDiscriminant) / (2 * a);
        float t2 = (-b + sqrtDiscriminant) / (2 * a);
        // Return the first positive time to collision
        if (t1 >= 0) return t1;
        if (t2 >= 0) return t2;
        // No future collision
        return -1;
    }
}