using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Bystander/Filter/Same Flock")]
public class BystanderSameFlockFilter : BystanderContextFilter
{
    public override List<Transform> Filter(BystanderAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            BystanderAgent itemAgent = item.GetComponent<BystanderAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) {
                filtered.Add(item);
            }
        }
        return filtered;
    } 
}
