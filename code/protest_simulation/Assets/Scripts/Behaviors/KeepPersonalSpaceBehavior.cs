
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Keep Personal Space Behavior")]
public class KeepPersonalSpaceBehavior : FilteredFlockBehavior
{
    float minForceScale = 0.1f; 
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        Vector2 comulativeAvoidance = new();
        foreach(FlockAgent neighbor in agent.visibleAgents)
        {
            Vector2 neighborToAgentVector = agent.transform.position - neighbor.transform.position ;
            if (neighborToAgentVector.magnitude <= flock.personalSpaceDistance)
            {
                Vector2 avoidNeighbor = neighborToAgentVector.normalized / (neighborToAgentVector.magnitude/flock.personalSpaceDistance);
                comulativeAvoidance += avoidNeighbor*minForceScale;
            }
        }
        return comulativeAvoidance;
    }
}