using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior {

    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(FlockAgent agent, Flock flock)
    {
        if(agent.allVisibleThings.Count == 0)
            return Vector2.zero;

        Vector2 cohesionMove = Vector2.zero;
        List<GameObject> filteredContext = (filter == null) ? agent.allVisibleThings : filter.Filter(agent, agent.allVisibleThings);
        foreach (GameObject item in filteredContext) {
            cohesionMove += (Vector2)item.transform.position;
        }       

        cohesionMove /= agent.allVisibleThings.Count;

        //offset
        cohesionMove -= (Vector2)agent.transform.position;
        cohesionMove = Vector2.SmoothDamp(agent.transform.up, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;
    }

    
}
