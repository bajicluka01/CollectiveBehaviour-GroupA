using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Flock/Filter/Agent Layer")]
public class AgentLayerFilter : ContextFilter {
    public LayerMask mask;

    public override List<GameObject> Filter(FlockAgent agent, List<GameObject> original) {
        List<GameObject> filtered = new();
        foreach(GameObject item in original) {
            if (mask == (mask | (1 << item.layer))) {
                filtered.Add(item);
            } 
        }
        
        return filtered;
    } 
    
}
