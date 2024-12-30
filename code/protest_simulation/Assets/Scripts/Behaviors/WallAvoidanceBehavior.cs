using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

[CreateAssetMenu(menuName = "Flock/Behavior/Wall Avoidance")]
public class WallAvoidanceBehavior : FlockBehavior
{
    readonly float theta = 0.9748f;
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock)
    {
        List<GameObject> walls = context.Where(e => e.tag.Equals("map")).ToList();
        GameObject lowestTTCWall = GetLowestTTCWall(agent, walls);

        if (lowestTTCWall != null)
        {
            Vector3 wij = lowestTTCWall.transform.position - agent.transform.position;
            Vector3 wu = CalculateWu(wij, agent);
            Vector3 wc = Vector3.Cross(wu, wij);
            wc.Normalize();
            // TODO: check if this is good
            // for the R parameter that is defined as the maximum possible distance between the 
            // float S = 1;
            // float R = 1;
            // for now i will just leave this as it is and implement other behaviours first
            // the s parameter needst to be calcluated for groups
            return wc;
        }

        return Vector2.zero;
    }

    public Vector3 CalculateWu(Vector3 wij, FlockAgent agent)
    {
        if (Vector2.Dot(wij, agent.PreviousMove) >= theta)
        {
            return Vector3.up;
        }
        else
        {
            return Vector3.Cross(wij, agent.PreviousMove);
        }
    }

    public GameObject GetLowestTTCWall(FlockAgent agent, List<GameObject> walls)
    {
        float minTTc = -1;
        GameObject closestWall = null;
        foreach (GameObject wall in walls)
        {
            float ttc = CalculateTimeToCollision(agent.transform.position, agent.PreviousMove,
                                                wall.transform.position, Vector2.zero,
                                                agent.ColiderRadius / 2, 0);
            if (ttc != (-1) && ((minTTc == (-1)) || (ttc < minTTc)))
            {
                minTTc = ttc;
                closestWall = wall;
            }
        }
        return closestWall;
    }

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
