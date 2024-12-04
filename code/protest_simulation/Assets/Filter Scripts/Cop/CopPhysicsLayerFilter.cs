using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Cop/Filter/Physics Layer")]
public class CopPhysicsLayerFilter : CopContextFilter {
    public LayerMask mask;

    public override List<Transform> Filter(CopAgent agent, List<Transform> original) {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original) {
            if (mask == (mask | (1 << item.gameObject.layer))) {
                filtered.Add(item);
            } 
        }
        return filtered;
    } 
    
}
