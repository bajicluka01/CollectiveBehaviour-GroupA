using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Leader/Filter/Physics Layer")]
public class LeaderPhysicsLayerFilter : LeaderContextFilter {
    public LayerMask mask;

    public override List<Transform> Filter(LeaderAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            if (mask == (mask | (1 << item.gameObject.layer))) {
                filtered.Add(item);
            } 
        }
        return filtered;
    } 
    
}
