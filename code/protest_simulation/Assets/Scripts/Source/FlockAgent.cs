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

    float desiredSpeed;
    public float DesiredSpeed
    {
        get {return desiredSpeed;}
        set
        {
            desiredSpeed = value;
        }
    }

    Vector2 previousMove;
    public Vector2 PreviousMove { get { return previousMove;}}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        agentCollider = GetComponent<Collider2D>();
        desiredSpeed = 19.3f;
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
        previousMove = velocity;
    }
}
