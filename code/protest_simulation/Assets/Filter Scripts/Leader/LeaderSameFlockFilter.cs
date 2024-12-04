using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Leader/Filter/Same Flock")]
public class LeaderSameFlockFilter : LeaderContextFilter
{
    public override List<Transform> Filter(LeaderAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            LeaderAgent itemAgent = item.GetComponent<LeaderAgent>();
            if (itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock) {
                filtered.Add(item);
            }
        }
        return filtered;
    } 
}
