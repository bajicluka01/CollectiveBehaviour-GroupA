using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior 
{
    public override Vector2 CalculateMove(FlockAgent agent, Flock flock) 
    {
        //no neighbors
        if(agent.allVisibleThings.Count == 0)
            return agent.transform.up;

        Vector2 alignmentMove = Vector2.zero;
        List<GameObject> filteredContext = (filter == null) ? agent.allVisibleThings : filter.Filter(agent, agent.allVisibleThings);
        foreach (GameObject item in filteredContext) 
        {
            alignmentMove += (Vector2)item.transform.transform.up;
        }       

        alignmentMove /= agent.allVisibleThings.Count;

        return alignmentMove;
    }
}
