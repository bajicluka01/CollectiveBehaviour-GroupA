using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Cop/Filter/Same Flock")]
public class CopSameFlockFilter : CopContextFilter
{
    public override List<Transform> Filter(CopAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            CopAgent itemAgent = item.GetComponent<CopAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) {
                filtered.Add(item);
            }
        }
        return filtered;
    } 
}
