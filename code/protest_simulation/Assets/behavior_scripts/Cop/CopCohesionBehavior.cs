using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cop/Behavior/Cohesion")]
public class CopCohesionBehavior : CopFilteredFlockBehavior {

    public override Vector2 CalculateMove(CopAgent agent, List<Transform> context, CopFlock flock) {
        if(context.Count == 0)
            return Vector2.zero;

        Vector2 cohesionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext) {
            cohesionMove += (Vector2)item.position;
        }       

        cohesionMove /= context.Count;

        //offset
        cohesionMove -= (Vector2)agent.transform.position;
        return cohesionMove;
    }

    
}
