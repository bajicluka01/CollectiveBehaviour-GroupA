using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum AgentRole
{
    Leader,
    Protester,
    Bystander,
    Police
}

public enum AgentState
{
    inMotion,
    Stationary
}

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    // agent characteristics
    float restlessness = 0f;

    float recruitmentTimer = 0f;
    float defectionTimer = 0f;

    float defectionProb = 0f;

    float recruitmentProb = 0f;

    //float recruitment_probability = 0.2f;
    //float defection_probability = 0.8f;

    public float Restlessness
    {
        get { return restlessness; }
    }

    AgentRole role = AgentRole.Bystander;
    public AgentRole Role
    {
        get { return role; }
        set
        {
            SetAgentRole(value);
        }
    }

    AgentState state = AgentState.Stationary;
    public AgentState State
    {
        get { return state; }
        set
        {
            state = value;
        }
    }


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
    public float ColiderRadius { get { return coliderRadius; } }
    float eyesightDistance = 24.0f;
    public float EyesightDistance
    {
        get { return eyesightDistance; }
        set
        {
            visualAngleChange = CalculateRayCastAngleChange(coliderRadius, value);
            eyesightDistance = value;
        }
    }

    float peripersonalDistance = 1.7f;
    public float PeripsersonalDistance
    {
        get { return peripersonalDistance; }
        set
        {
            personalSpaceAngleChange = CalculateRayCastAngleChange(coliderRadius, value);
            peripersonalDistance = value;
        }
    }

    float fov = 60f;
    public float Fov
    {
        get { return fov; }
        set
        {
            fov = value;
        }
    }

    float visualAngleChange;
    float personalSpaceAngleChange;

    Vector2 previousMove;
    public Vector2 PreviousMove { get { return previousMove; } }

    public bool showFOV;

    Rigidbody2D agentRigidBody;

    public List<GameObject> allVisibleThings;
    public List<FlockAgent> visibleAgents; 
    public List<FlockAgent> visibleBystanders;
    public List<FlockAgent> visibleProtesters;
    public List<FlockAgent> visibleLeaders; 

    void Start()
    {
        agentCollider = GetComponent<Collider2D>();
        desiredSpeed = 19.3f;
        CircleCollider2D colider = GetComponent<CircleCollider2D>();
        coliderRadius = colider.radius;
        visualAngleChange = CalculateRayCastAngleChange(coliderRadius, eyesightDistance);
        personalSpaceAngleChange = CalculateRayCastAngleChange(coliderRadius, peripersonalDistance);
        SetAgentRole(role);
        agentRigidBody = GetComponent<Rigidbody2D>();
        restlessness = UnityEngine.Random.Range(0f, 1f);
    }

    public void CustomUpdate()
    {
        
        allVisibleThings = GroupContext.GetDistinctGameObjectFromHits(GroupContext.GetHits(GetVisibleAgents()));
        visibleAgents = GroupContext.GetFlockAgents(allVisibleThings);
        visibleBystanders = GroupContext.GetBystanders(visibleAgents);
        visibleProtesters = GroupContext.GetProtesters(visibleAgents);
        visibleLeaders = GroupContext.GetLeaders(visibleAgents);
        CalculateAgentState();
        // CalculateContagion();
    }

    void CalculateAgentState()
    {
        
        if (state == AgentState.Stationary)
        {
            restlessness += 0.04f * Time.deltaTime;
            if (restlessness > 1.0f)
            {
                state = AgentState.inMotion;
                ResetRestlessness();
            }
        }

        if (previousMove != Vector2.zero)
        {
            state = AgentState.inMotion;
        }
    }

    void CalculateContagion()
    {
        // increase timers
        if (role == AgentRole.Protester)
        {
            defectionTimer += 0.1f * Time.deltaTime;
        }
        else if (role == AgentRole.Bystander)
        {
            recruitmentTimer += 0.1f * Time.deltaTime;
        }

        // change state with a certain probability
        if (defectionTimer > 1.0f)
        {
            defectionTimer = UnityEngine.Random.Range(0, 0.5f);
            defectionProb = DefectionProbability(visibleProtesters.Count(), visibleBystanders.Count(), visibleLeaders.Count());
            if (UnityEngine.Random.Range(0,100f) < defectionProb * 100)
            {
                SetAgentRole(AgentRole.Bystander);
            }
        }
        if (recruitmentTimer > 1.0f)
        {
            recruitmentTimer = UnityEngine.Random.Range(0, 0.5f);
            recruitmentProb = RecruitmentProbability(visibleProtesters.Count(), visibleBystanders.Count(), visibleLeaders.Count());
            if (UnityEngine.Random.Range(0,100f) < recruitmentProb * 100)
            {
                SetAgentRole(AgentRole.Protester);
            }
        }
    }

    float DefectionProbability(int protesterCount, int bystanderCount, int leaderCount)
    {
        //TODO: add fuction that calculates defection probability based on num_protesters and num_bystanders
        return 0.8f;
    }

    float RecruitmentProbability(int protesterCount, int bystanderCount, int leaderCount)
    {
        //TODO: add fuction that calculates defection probability based on num_protesters and num_bystanders
        return 0.2f;
    }

    public void ResetRestlessness()
    {
        restlessness = UnityEngine.Random.Range(0, 0.5f);
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
        if (velocity != Vector2.zero)
        {
            transform.up = velocity;
            agentRigidBody.constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            agentRigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        agentRigidBody.linearVelocity = velocity;
        previousMove = velocity;
    }

    float CalculateRayCastAngleChange(float humanRadius, float desiredDistance)
    {
        float desiredDistanceSquared = Mathf.Pow(desiredDistance, 2);
        float cosTheta = (2 * desiredDistanceSquared - Mathf.Pow(humanRadius * 2, 2)) / (2 * desiredDistanceSquared);
        float angle = Mathf.Acos(cosTheta) * Mathf.Rad2Deg;
        return angle / 4;
    }

    public List<(RaycastHit2D, Vector2)> GetVisibleAgents()
    {
        List<(RaycastHit2D, Vector2)> visibleObjects = new();
        Vector2 direction = transform.rotation * Vector2.up * eyesightDistance;
        float angle = 0f;
        while (angle < fov)
        {
            if (angle == 0f)
            {
                visibleObjects.Add((Physics2D.Raycast(transform.position, direction, eyesightDistance), direction));
            }
            else
            {
                Vector2 positiveVector = Numbers.Rotate2D(direction, angle);
                Vector2 negativeVector = Numbers.Rotate2D(direction, -angle);
                visibleObjects.Add((Physics2D.Raycast(transform.position, positiveVector, eyesightDistance), positiveVector));
                visibleObjects.Add((Physics2D.Raycast(transform.position, negativeVector, eyesightDistance), negativeVector));
            }
            angle += visualAngleChange;
        }
        direction.Normalize();
        direction *= peripersonalDistance;
        while (angle < 180)
        {
            // TODO: Draw short sticks
            Vector2 positiveVector = Numbers.Rotate2D(direction, angle);
            Vector2 negativeVector = Numbers.Rotate2D(direction, -angle);
            visibleObjects.Add((Physics2D.Raycast(transform.position, positiveVector, peripersonalDistance), positiveVector));
            visibleObjects.Add((Physics2D.Raycast(transform.position, negativeVector, peripersonalDistance), negativeVector));
            angle += personalSpaceAngleChange / 4;
        }
        return visibleObjects;
    }

    // note that the lookaround angle should be maximum 180 degrees 
    // the lookaround angle is in degrees
    // 180 -> 360
    public void LookAround(float lookAroundAngle)
    {
        List<RaycastHit2D> hits = new();
        Vector2 direction = transform.rotation * Vector2.up * eyesightDistance;
        float angle = 0f;
        while (angle < lookAroundAngle)
        {
            if (angle == 0f) { hits.Add(Physics2D.Raycast(transform.position, direction, eyesightDistance)); }
            else
            {
                Vector2 positiveVector = Numbers.Rotate2D(direction, angle);
                Vector2 negativeVector = Numbers.Rotate2D(direction, -angle);
                hits.Add(Physics2D.Raycast(transform.position, positiveVector, eyesightDistance));
                hits.Add(Physics2D.Raycast(transform.position, negativeVector, eyesightDistance));
            }
            angle += visualAngleChange;
        }
        visibleAgents =  GroupContext.GetFlockAgents(GroupContext.GetDistinctGameObjectFromHits(hits));
        visibleProtesters = GroupContext.GetProtesters(visibleAgents);
        visibleBystanders = GroupContext.GetBystanders(visibleAgents);
        visibleLeaders = GroupContext.GetLeaders(visibleAgents);
    }

    public void SetAgentRole(AgentRole newRole)
    {
        role = newRole;
        switch (newRole)
        {
            case AgentRole.Leader:
                ChangeBodyColor(Color.green);
                break;
            case AgentRole.Protester:
                ChangeBodyColor(Color.red);
                break;
            case AgentRole.Bystander:
                ChangeBodyColor(Color.white);
                break;
            case AgentRole.Police:
                ChangeBodyColor(Color.blue);
                break;
            default:
                throw new System.Exception("Agent does not have a role");
        }
    }

    public void DrawHits(List<(RaycastHit2D, Vector2)> directionsAndHits)
    {
        foreach ((RaycastHit2D hit, Vector2 direction) in directionsAndHits)
        {
            if (hit)
            {
                Debug.DrawRay(transform.position, hit.collider.transform.position - transform.position, Color.green);
            }
            else
            {
                if (direction.magnitude > peripersonalDistance + 0.1f)
                {
                    Debug.DrawRay(transform.position, direction, Color.red);
                }
                else
                {
                    Debug.DrawRay(transform.position, direction, Color.yellow);
                }
            }

        }
    }
}


