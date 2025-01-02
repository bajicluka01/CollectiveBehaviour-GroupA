using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

        //TODO 
        //we need some logic to make the agent give up if it can't reach desired position (either because a building or barricade is in the way)

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
            float someConst = distance/2;
            return GenerateRandomPositionInsideRing(distance, distance+someConst, center);
        }
    }

    //returns true if point (x,y) lies inside a building, false otherwise
    bool isInsideBuilding(float x, float y) 
    {
        //these two methods should be more efficient than the loop, but for some reason I can' get them to work)

        //Collider[] intersecting = Physics.OverlapSphere(new Vector3(x,y,0), 0.01f);
        //if (intersecting.Length != 0)
        //    Debug.Log(intersecting.Length);
        //return intersecting.Length != 0;

        //if (Physics.CheckSphere(new Vector3 (x,y,0), 0.01f))
        //    Debug.Log(x+" "+ y);

        //List<GameObject> walls = agent.allVisibleThings.Where(e => e.tag.Equals("map")).ToList();
        List<GameObject> walls = GameObject.FindGameObjectsWithTag("map").ToList();
        foreach (GameObject wall in walls)
        {
            if (Vector3.Distance(new Vector3(x,y,0), wall.transform.position) <= 0.1f)
                return true;
        }

        return false;
    }

    Vector2 GenerateRandomPositionInsideRing(float minDistance, float maxDistance, Vector3 center)
    {
        //just in case we have the worst luck imaginable (with the Random generator)
        float maxIter = 1000;
        
        for (float i = 0f; i < maxIter; i++) 
        {
            // Generate a random angle
            float angle = Random.Range(0f, 2f * Mathf.PI);
            // Generate a random distance from 0 to maxDistance
            float randomDistance = Random.Range(minDistance, maxDistance);
            
            // Calculate the random position using polar coordinates
            float x = center.x + randomDistance * Mathf.Cos(angle);
            float y = center.y + randomDistance * Mathf.Sin(angle);

            //check if the position is acceptable
            if (!isInsideBuilding(x,y))
                return new Vector2(x,y);
        }
        
        //not quite sure what to do here, maybe simply no movement?
        return new Vector2(center.x, center.y);
    }
}