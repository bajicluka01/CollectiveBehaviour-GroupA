using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ProtesterContextFilter : ScriptableObject {
    public abstract List<Transform> Filter (ProtesterAgent agent, List<Transform> original);
}
