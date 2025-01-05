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
                agent.DesiredPosition = leader.DesiredPosition;
            else
                agent.DesiredPosition = leader.transform.position;
        }
        else if (agent.LeaderIndex > 0)
        {
            List<FlockAgent> visibleLeadingProtesters = agent.GetListOfAgentsWithIndex(agent.LeaderIndex - 1, agent.visibleProtesters);
            if (visibleLeadingProtesters.Count == 0)
                return Vector2.zero;
            if (isDesiredPositionLeaderDesiredPosition)
                agent.DesiredPosition = visibleLeadingProtesters.First().DesiredPosition;
            else
            {
                Vector3 center = visibleLeadingProtesters.Select(agent => agent.transform.position).Aggregate((agg, pos) => agg + pos) / visibleLeadingProtesters.Count;
                // TODO: this needs to be tested more
                Vector2 averageHeading = visibleLeadingProtesters.Select(agent => agent.PreviousMove).Aggregate((agg, pos) => agg + pos)*20 / visibleLeadingProtesters.Count;
                agent.DesiredPosition = center + new Vector3(averageHeading.x, averageHeading.y, 0);
            }
            
        }
        else
            return Vector2.zero;
        if(agent.OnDesiredPosition())
            agent.DesiredPosition = Vector2.zero;
        if(agent.DesiredPosition == Vector3.zero)
            return Vector2.zero;
        return EndPositionSeekingBehavior.CalculateDesiredPositionWithDesiredSpeedVector(agent);
        
    }
}