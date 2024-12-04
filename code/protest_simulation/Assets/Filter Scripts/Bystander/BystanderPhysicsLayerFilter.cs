using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Bystander/Filter/Physics Layer")]
public class BystanderPhysicsLayerFilter : BystanderContextFilter {
    public LayerMask mask;

    public override List<Transform> Filter(BystanderAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            if (mask == (mask | (1 << item.gameObject.layer))) {
                filtered.Add(item);
            } 
        }
        return filtered;
    } 
    
}
