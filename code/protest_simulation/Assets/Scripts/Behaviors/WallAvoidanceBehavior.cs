using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Linq;

[CreateAssetMenu(menuName = "Flock/Behavior/Wall Avoidance")]
public class WallAvoidanceBehavior : FlockBehavior
{
    readonly float theta = 0.9748f;

    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        List<GameObject> walls = agent.allVisibleThings.Where(e => e.tag.Equals("map")).ToList();
        GameObject lowestTTCWall = GetLowestTTCWall(agent, walls);

        if(agent.WallAvoidTimer > 0.5f){
            agent.WallAvoidTimer = 0.0f;
            agent.IncreaseWallTimer = false;

            //TODO
            //not sure if this is needed
            agent.WallAvoidDirection = Vector2.zero;
        }

        if(agent.IncreaseWallTimer){
            agent.WallAvoidTimer += Time.deltaTime;
        }

        // not sure if 2nd condition is necessary
        if (lowestTTCWall != null && agent.State != AgentState.Stationary)
        {

            Vector3 wij = lowestTTCWall.transform.position - agent.transform.position;
            Vector3 wu = CalculateWu(wij, agent);

            //not sure if this is OK
            if (wu.z < 0)
            {
                wu.z *= -1;
            }

            Vector3 wc = Vector3.Cross(wu, wij);
            Vector2 wc2 = new Vector2(wc.x, wc.y);
            wc2.Normalize();
            //wc.Normalize();
            Vector2 wc2final = Vector2.Perpendicular(wc2);
            wc2final.Normalize();
            wij.Normalize();

            //Debug.Log(wij+ " " + wu);

            Vector2 wij2 = new Vector2(wij.x, wij.y);

            // TODO: check if this is good
            // for the R parameter that is defined as the maximum possible distance between the 
            // float S = 1;
            // float R = 1;
            // for now i will just leave this as it is and implement other behaviours first
            // the s parameter needst to be calcluated for groups
            float S = 1.0f;
            float R = 10.0f;
            //Vector2 fc = S * (1-wij2.sqrMagnitude/R) * wc2final;
            //Vector2 fc = S * Mathf.Abs((1-wij.sqrMagnitude/R)) * wc;
            Vector2 fc = S * (1-wij.sqrMagnitude/R) * wc;
            //Debug.Log(wij2.magnitude+" "+ fc+" "+ wc2final);

            //Debug.Log(wu+" "+ wc+ " "+wij+ " "+ Vector2.Perpendicular(wij)+" " +fc);


            //normalization doesn't seem to change anything
            //fc.Normalize();

            if(agent.WallAvoidTimer == 0.0f){
                agent.IncreaseWallTimer = true;
            }

            if(fc != Vector2.zero){
                agent.WallAvoidDirection = fc;
                //agent.WallAvoidTimer = 0.0f;
            }

            if(agent.IncreaseWallTimer){
                fc = agent.WallAvoidDirection;
            }

            return fc;
            //return Vector2.Perpendicular(wij2);
        }// else{
           // agent.WallAvoidDirection = Vector2.zero;
        //}

        if(agent.IncreaseWallTimer){
            return agent.WallAvoidDirection;
        }

        return Vector2.zero;
    }

    public Vector3 CalculateWu(Vector3 wij, FlockAgent agent)
    {
        Vector2 wij2 = new Vector2(wij.x, wij.y);

        if (Vector2.Dot(wij2, agent.PreviousMove) * Mathf.Deg2Rad >= theta)
        {
            return Vector3.up;
            //return Vector3.Cross(wij, agent.PreviousMove);
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
