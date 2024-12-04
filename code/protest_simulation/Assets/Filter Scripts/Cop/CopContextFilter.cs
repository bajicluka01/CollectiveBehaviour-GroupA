using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CopContextFilter : ScriptableObject {
    public abstract List<Transform> Filter (CopAgent agent, List<Transform> original);
}
