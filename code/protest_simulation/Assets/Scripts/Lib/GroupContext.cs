

using System;
using System.Collections.Generic;
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

    // TODO: to be tested
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

    // TODO: to be tested
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
}