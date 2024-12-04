using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Protester/Behavior/SteeredCohesion")]
public class ProtesterSteeredCohesionBehavior : ProtesterFilteredFlockBehavior {

    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(ProtesterAgent agent, List<Transform> context, ProtesterFlock flock) {
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
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }

    
}
