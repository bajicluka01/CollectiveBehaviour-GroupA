using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Bystander Leader Follow Behavior")]
public class BystanderLeaderFollowBehavior : FlockBehavior
{
    float paddingDistance = 5;
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        return GoToTheSideVector(agent);
    }

    public Vector2 GoToTheSideVector(FlockAgent agent)
    {
        Vector2 result = Vector2.zero;
        FlockAgent protester = NearestProtester(agent);
        if (protester)
        {
            Vector3 agentToProtester = protester.transform.position - agent.transform.position;
            float distance = agentToProtester.magnitude;
            if (distance < paddingDistance)
            {
                if (Vector2.zero != protester.PreviousMove)
                    result += agent.directionalPreference * Vector2.Perpendicular(protester.PreviousMove);
                else
                    result += Vector2.zero;
            }
        }
        return result;
    }

    public FlockAgent NearestProtester(FlockAgent bystander)
    {
        return bystander.visibleProtesters.OrderBy(obj => Vector3.Distance(obj.transform.position, bystander.transform.position)).FirstOrDefault();
    }
}