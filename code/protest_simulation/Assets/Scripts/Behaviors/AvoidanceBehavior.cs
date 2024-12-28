using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent agent, List<GameObject> context, Flock flock) 
    {
        if(context.Count == 0)
            return Vector2.zero;

        Vector2 avoidanceMove = Vector2.zero;
        int nAvoid = 0;
        List<GameObject> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (GameObject item in filteredContext) {
            if (Vector2.SqrMagnitude(item.transform.position-agent.transform.position) < flock.SquareAvoidanceRadius) {
                nAvoid++;
                avoidanceMove += (Vector2)(agent.transform.position-item.transform.position);
            }
            
        }       
        if (nAvoid > 0) 
            avoidanceMove /= nAvoid;
        
        return avoidanceMove;
    }
}
