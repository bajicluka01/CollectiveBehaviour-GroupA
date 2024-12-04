using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Protester/Filter/Same Flock")]
public class ProtesterSameFlockFilter : ProtesterContextFilter
{
    public override List<Transform> Filter(ProtesterAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            ProtesterAgent itemAgent = item.GetComponent<ProtesterAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) {
                filtered.Add(item);
            }
        }
        return filtered;
    } 
}
