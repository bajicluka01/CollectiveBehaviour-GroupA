using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Leader/Behavior/Alignment")]
public class LeaderAlignmentBehavior : LeaderFilteredFlockBehavior {
    public override Vector2 CalculateMove(LeaderAgent agent, List<Transform> context, LeaderFlock flock) {
        //no neighbors
        if(context.Count == 0)
            return agent.transform.up;

        Vector2 alignmentMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext) {
            alignmentMove += (Vector2)item.transform.up;
        }       

        alignmentMove /= context.Count;

        return alignmentMove;
    }
}
