using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CopAgent : MonoBehaviour {
    CopFlock agentFlock;
    public CopFlock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        agentCollider = GetComponent<Collider2D>();
    }

    public void Initialize(CopFlock flock) {
        agentFlock = flock;
    }

    public void Move(Vector2 velocity) {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;

    }
}
