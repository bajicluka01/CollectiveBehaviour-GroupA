using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Cop/Behavior/Stay In Radius")]
public class CopStayInRadiusBehavior : CopBehavior {

    public Vector2 center;
    public float radius = 100f;
    
    public override Vector2 CalculateMove(CopAgent agent, List<Transform> context, CopFlock flock) {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude /radius;
        if (t < 0.9f) {
            return Vector2.zero;
        }
        return centerOffset*t*t;
    }
}
