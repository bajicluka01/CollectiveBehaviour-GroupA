using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter
{
    public override List<GameObject> Filter(FlockAgent agent, List<GameObject> original) {
        List<GameObject> filtered = new();
        foreach(GameObject item in original) {
            FlockAgent itemAgent = item.transform.GetComponent<FlockAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) {
                filtered.Add(item);
            }
        }
        
        return filtered;
    } 
}
