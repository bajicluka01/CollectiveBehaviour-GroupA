using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cop/Behavior/Avoidance")]
public class CopAvoidanceBehavior : CopFilteredFlockBehavior
{
    public override Vector2 CalculateMove(CopAgent agent, List<Transform> context, CopFlock flock) {
        if(context.Count == 0)
            return Vector2.zero;

        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext) {
            if (Vector2.SqrMagnitude(item.position-agent.transform.position) < flock.SquareAvoidanceRadius) {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position-item.position);
            }
            
        }       

        if (nAvoid > 0) 
            avoidanceMove /= nAvoid;
        
        return avoidanceMove;
    }
}
