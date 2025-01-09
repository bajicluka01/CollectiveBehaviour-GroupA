using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Protester Leader Follow Behavior")]
public class ProtesterLeaderFollowBehavior : FlockBehavior
{
    public bool isDesiredPositionLeaderDesiredPosition = false;
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        agent.DesiredPosition = Vector3.zero;
        if (agent.visibleLeaders.Count > 0)
        {
            FlockAgent leader = agent.visibleLeaders.First();
            if (isDesiredPositionLeaderDesiredPosition)
                return leader.DesiredPosition;
            else
                return CalculateDesiredPosition(leader.transform.position, leader.PreviousMove, agent);
        }
        else if (agent.LeaderIndex > 0)
        {
            int lowestVisibleIndex = agent.GetLowestIndexOfVisibleAgents(agent.visibleProtesters);
            if (lowestVisibleIndex == -1)
                return Vector2.zero;
            List<FlockAgent> visibleLeadingProtesters = agent.GetListOfAgentsWithIndex(lowestVisibleIndex, agent.visibleProtesters);
            if (visibleLeadingProtesters.Count == 0)
                return Vector2.zero;
            if (isDesiredPositionLeaderDesiredPosition)
                return visibleLeadingProtesters.First().DesiredPosition;
            else
            {
                Vector3 center = visibleLeadingProtesters.Select(agent => agent.transform.position).Aggregate((agg, pos) => agg + pos) / visibleLeadingProtesters.Count;
                Vector2 averageHeading = visibleLeadingProtesters.Select(agent => agent.PreviousMove).Aggregate((agg, pos) => agg + pos)*20 / visibleLeadingProtesters.Count;
                return CalculateDesiredPosition(center, averageHeading, agent);
            }
        }
        else
            return Vector2.zero;
    }

    public static Vector2 CalculateDesiredPosition(Vector3 center, Vector2 heading, FlockAgent agent)
    {
        Vector2 centerToAgnet = agent.transform.position - center;
        float dotProd = Vector2.Dot(centerToAgnet, heading);
        if (heading == Vector2.zero)
            return Vector2.zero;
        if (dotProd < 0)
            // leader is not coming tward you
            return (center - agent.transform.position).normalized * agent.DesiredSpeed;
        else
            return (center - agent.transform.position).normalized * 0.01f;
    }
}