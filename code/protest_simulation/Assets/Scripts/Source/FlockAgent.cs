using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider2D agentCollider;
    public Collider2D AgentCollider { get { return agentCollider; } }

    Vector3 desiredPosition;
    public Vector3 DesiredPosition
    {
        get { return desiredPosition; }
        set
        {
            desiredPosition = value;
        }
    }

    float desiredSpeed;
    public float DesiredSpeed
    {
        get { return desiredSpeed; }
        set
        {
            desiredSpeed = value;
        }
    }

    float coliderRadius;

    float eyesightDistance = 24.0f;
    public float EyesightDistance
    {
        get { return eyesightDistance; }
        set
        {
            angleChange = CalculateRayCastAngleChange(coliderRadius,eyesightDistance);
            eyesightDistance = value;
        }
    }

    float fov = 60f;
    public float Fov
    {
        get {return fov;}
        set
        {
            fov = value;
        }
    }
    
    float angleChange;

    Vector2 previousMove;
    public Vector2 PreviousMove { get { return previousMove; } }

    public bool showFOV;

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        desiredSpeed = 19.3f;
        CircleCollider2D colider = GetComponent<CircleCollider2D>();
        coliderRadius = colider.radius;
        angleChange = CalculateRayCastAngleChange(coliderRadius,eyesightDistance);
    }

    public void ChangeBodyColor(Color color)
    {
        Transform body = transform.Find("Capsule");
        SpriteRenderer sr = body.GetComponent<SpriteRenderer>();
        sr.color = color;   
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

    float CalculateRayCastAngleChange(float humanRadius, float desiredDistance)
    {
        float desiredDistanceSquared = Mathf.Pow(desiredDistance, 2);
        float cosTheta = (2 * desiredDistanceSquared - Mathf.Pow(humanRadius * 2, 2)) / (2 * desiredDistanceSquared);
        float angle = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
        return angle/4;
    }

    public List<(RaycastHit2D, Vector2)> GetVisibleAgents()
    {
        List<(RaycastHit2D, Vector2)> visibleObjects = new(); 
        Vector2 direction = transform.rotation*Vector2.up*eyesightDistance;
        visibleObjects.Add((Physics2D.Raycast(transform.position,direction,eyesightDistance),direction));
        float angle = 0f;
        while (angle < fov)
        {
            angle = angle + angleChange;
            Vector2 positiveVector = Numbers.Rotate2D(direction,angle);
            Vector2 negativeVector = Numbers.Rotate2D(direction,-angle);
            visibleObjects.Add((Physics2D.Raycast(transform.position, positiveVector, eyesightDistance), positiveVector));
            visibleObjects.Add((Physics2D.Raycast(transform.position, negativeVector, eyesightDistance), negativeVector));
        }
        return visibleObjects;
    }

    // TODO: delete this this is only for testing
    private void FixedUpdate() 
    {
        List<(RaycastHit2D, Vector2)> hits = GetVisibleAgents();
        if (showFOV) 
        {
            DrawHits(hits);
        }
    }


    public void DrawHits(List<(RaycastHit2D,Vector2)> directionsAndHits)
    {
        foreach((RaycastHit2D hit, Vector2 direction) in directionsAndHits)
        {
            if (hit)
            {
                Debug.DrawRay(transform.position, hit.collider.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, direction, Color.red);
            }

        }
    }
}


