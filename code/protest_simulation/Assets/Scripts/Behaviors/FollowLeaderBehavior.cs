using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UIElements;

// this class describes how protestors react to the leader that is present in the simulation
[CreateAssetMenu(menuName = "Flock/Behavior/Protester follow leader")]
public class FollowLeaderBehavior : FlockBehavior
{
    float const1 = 0.1f;
    float const2 = 0.7f;

    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        if (agent.leader)
        {
            FlockAgent leader = agent.leader;
            Vector2 part1 = ProtesterLeaderFollowBehavior.CalculateDesiredPosition(leader.transform.position, agent.leader.PreviousMove, agent) * const1;
            Vector2 part2 = MakeWayForLeader(leader, agent) * const2;
            return part1 + part2;
        }
        return Vector2.zero;
    }

    public Vector2 MakeWayForLeader(FlockAgent leader, FlockAgent agent)
    {
        if (leader.visibleAgents.Contains(agent))
        {
            Vector3 leaderToAgent = agent.transform.position - leader.transform.position;
            Vector3 leaderPreviousMove = leader.PreviousMove;
            Vector3 crossProduct = Vector3.Cross(leaderPreviousMove, leaderToAgent);
            float dotProd = Vector2.Dot(leaderToAgent, leaderPreviousMove);
            if (dotProd > 0)
            {
                if (crossProduct.z > 0)
                    return Vector2.Perpendicular(leaderToAgent);
                else
                    return Vector2.Perpendicular(-leaderToAgent);
            }
        }
        return Vector2.zero;

    }
}

