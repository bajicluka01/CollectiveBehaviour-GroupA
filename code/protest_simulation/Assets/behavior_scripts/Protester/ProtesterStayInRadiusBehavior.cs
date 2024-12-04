using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Protester/Behavior/Stay In Radius")]
public class ProtesterStayInRadiusBehavior : ProtesterBehavior {

    public Vector2 center;
    public float radius = 100f;
    
    public override Vector2 CalculateMove(ProtesterAgent agent, List<Transform> context, ProtesterFlock flock) {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude /radius;
        if (t < 0.9f) {
            return Vector2.zero;
        }
        return centerOffset*t*t;
    }
}
