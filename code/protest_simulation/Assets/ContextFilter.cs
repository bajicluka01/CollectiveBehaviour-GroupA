using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ContextFilter : ScriptableObject {
    public abstract List<Transform> Filter (FlockAgent agent, List<Transform> original);
}
