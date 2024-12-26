using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ContextFilter : ScriptableObject {
    public abstract List<GameObject> Filter (FlockAgent agent, List<GameObject> original);
}
