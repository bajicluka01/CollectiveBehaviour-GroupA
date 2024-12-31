using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GroupContext
{

    public static Vector3 GetLeaderPosition(List<GameObject> context)
    {
        foreach(GameObject obj in context)
        {
            FlockAgent agent= obj.GetComponent<FlockAgent>();
            if(agent != null && agent.Role == AgentRole.Leader)
            {
                return agent.transform.position;
            }
        }
        return Vector3.zero;
    }

    public static bool LeaderPositionInContext(List<GameObject> context)
    {
        foreach(GameObject obj in context)
        {
            FlockAgent agent= obj.GetComponent<FlockAgent>();
            if(agent != null && agent.Role == AgentRole.Leader)
            {
                return true;
            }
        }
        return false;
    }

    public static List<FlockAgent> GetGroupMembers(FlockAgent agent, List<GameObject> context)
    {
        List<FlockAgent> groupMembers = new();
        foreach(GameObject obj in context)
        {
            FlockAgent visibleAgent = obj.GetComponent<FlockAgent>();
            if(visibleAgent != null && Vector2.Distance(visibleAgent.PreviousMove, agent.PreviousMove) <= 0.2)
            {
                groupMembers.Add(visibleAgent);
            }
        }
        return groupMembers;
    }

    public static List<FlockAgent> GetNonGroupMembers(FlockAgent agent, List<GameObject> context)
    {
        List<FlockAgent> nonMembers = new();
        foreach(GameObject obj in context)
        {
            FlockAgent visibleAgent = obj.GetComponent<FlockAgent>();
            if(visibleAgent != null && Vector2.Distance(visibleAgent.PreviousMove, agent.PreviousMove) > 0.2)
            {
                nonMembers.Add(visibleAgent);
            }
        }
        return nonMembers;
    }

    public static List<RaycastHit2D> GetHits(List<(RaycastHit2D, Vector2)> values)
    {
        return values.Where(pair => pair.Item1).Select((pair) => pair.Item1).ToList();

    }
    
    public static List<GameObject> GetDistinctGameObjectFromHits(List<RaycastHit2D> hits)
    {
        return hits.Select(e => e.collider.gameObject).Distinct().ToList();
    }

    public static List<FlockAgent> GetFlockAgents(List<GameObject> distinctGameObjects)
    {
        return distinctGameObjects.Where(e => e.tag.Equals("agent")).Select(e => e.GetComponent<FlockAgent>()).ToList();
    }

    public static List<FlockAgent> GetBystanders(List<FlockAgent> allAgents)
    {
        return allAgents.Where(agent => agent.Role == AgentRole.Bystander).ToList();
    }
    public static List<FlockAgent> GetProtesters(List<FlockAgent> allAgents)
    {
        return allAgents.Where(agent => agent.Role == AgentRole.Protester).ToList();
    }
    public static List<FlockAgent> GetLeaders(List<FlockAgent> allAgents)
    {
        return allAgents.Where(agent => agent.Role == AgentRole.Leader).ToList();
    }
}