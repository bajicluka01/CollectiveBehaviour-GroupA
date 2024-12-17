using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour {
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    Vector3 desiredPosition;
    public Vector3 DesiredPosition 
    {
        get {return desiredPosition;} 
        set 
        {
            desiredPosition = value;
        }
    }

    float desiredVelocity;
    public float DesiredVelocity
    {
        get {return desiredVelocity;}
        set
        {
            desiredVelocity = value;
        }
    }

    Vector2 previousVelocity;
    public Vector2 PreviousVelocity { get { return previousVelocity;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        agentCollider = GetComponent<Collider2D>();
        desiredVelocity = 169.3f;
    }

    public void Initialize(Flock flock) 
    {
        agentFlock = flock;
    }

    public bool OnDesiredPosition() 
    {
        return Vector3.Distance(transform.position, desiredPosition) < 0.1;
    }

    public void Move(Vector2 velocity) 
    {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
        
        previousVelocity = velocity;
        // Vector3.Lerp(exVector, newVector, time);
    }
}
