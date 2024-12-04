using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class LeaderContextFilter : ScriptableObject {
    public abstract List<Transform> Filter (LeaderAgent agent, List<Transform> original);
}
