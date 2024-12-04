using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Protester/Filter/Physics Layer")]
public class ProtesterPhysicsLayerFilter : ProtesterContextFilter {
    public LayerMask mask;

    public override List<Transform> Filter(ProtesterAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            if (mask == (mask | (1 << item.gameObject.layer))) {
                filtered.Add(item);
            } 
        }
        return filtered;
    } 
    
}
