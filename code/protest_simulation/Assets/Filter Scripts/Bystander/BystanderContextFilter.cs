using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BystanderContextFilter : ScriptableObject {
    public abstract List<Transform> Filter (BystanderAgent agent, List<Transform> original);
}
