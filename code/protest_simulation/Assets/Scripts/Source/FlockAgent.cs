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

    float coliderRadius = 2;
    public float eyesightDistance = 24.0f;
    public float fov = 60f;

    Vector2 previousMove;
    public Vector2 PreviousMove { get { return previousMove; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        desiredSpeed = 19.3f;
        CircleCollider2D colider = GetComponent<CircleCollider2D>();
        coliderRadius = colider.radius;
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

    float CalculateRayCastAngle(float humanRadius, float desiredDistance)
    {
        float desiredDistanceSquared = Mathf.Pow(desiredDistance, 2);
        float cosTheta = (2 * desiredDistanceSquared - Mathf.Pow(humanRadius * 2, 2)) / (2 * desiredDistanceSquared);
        float angle = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
        return angle/4;
    }

    private void FixedUpdate()
    {
        float angleChange = CalculateRayCastAngle(coliderRadius,eyesightDistance);
        Vector2 direction = Vector2.up*eyesightDistance;
        Vector3 direction3d = new Vector3(direction.x,direction.y,0);
        float angle = 0f;
        (Vector2,RaycastHit2D)[] directionsAndHits = new (Vector2,RaycastHit2D)[1] {
                (direction, Physics2D.Raycast(transform.position,direction,eyesightDistance))};
        DrawHits(directionsAndHits);
        while (angle < fov)
        {
            angle = angle + angleChange;
            Vector3 temp3Direction = Quaternion.Euler(0,0,angle) * direction3d;
            Vector2 new2Direction = new(temp3Direction.x, temp3Direction.y); 
            temp3Direction = Quaternion.Euler(0,0,-angle) * direction3d;
            Vector2 negativeDirection = new(temp3Direction.x,temp3Direction.y);
            Debug.Log(negativeDirection);
            directionsAndHits = new (Vector2,RaycastHit2D)[2] {
                (new2Direction, Physics2D.Raycast(transform.position,new2Direction,eyesightDistance)), 
                (negativeDirection, Physics2D.Raycast(transform.position,negativeDirection,eyesightDistance))
            };
            DrawHits(directionsAndHits);
        }
    }

    void DrawHits((Vector2, RaycastHit2D)[] directionsAndHits)
    {
        foreach((Vector2 direction, RaycastHit2D hit) in directionsAndHits)
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


