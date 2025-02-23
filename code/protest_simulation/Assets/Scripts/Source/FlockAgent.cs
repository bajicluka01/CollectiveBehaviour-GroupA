using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    Stationary,
    HerdMode,
    Flocking
}

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    // agent characteristics
    float restlessness = 0f;
    float restlessnessUpperbound = 1f;
    
    // righthanded 
    public int directionalPreference;

    // MEANING OF INDEX:
    // -1 -> no leader visible
    // 0 -> leader is visible to the agent
    // 1 -> there is one agent between the leader and the agent
    // 2 -> there are two agents between the leader and the agent
    // ...
    // this creates a hierarchical structure between the agents
    int leaderIndex = -1;
    public FlockAgent leader;
    public int LeaderIndex
    {
        get { return leaderIndex; }
    }
    float herdCooldown = 0;
    public float HerdCooldown
    {
        get { return herdCooldown; }
    }

    float recruitmentTimer = 0f;
    float defectionTimer = 0f;

    float defectionProb = 0f;

    float recruitmentProb = 0f;

    // wall avoidance
    float wallAvoidTimer = 0.0f;
    bool increaseWallTimer = false;
    Vector2 wallAvoidDirection;

    public float WallAvoidTimer
    {
        get { return wallAvoidTimer; }
        set { wallAvoidTimer = value; }
    }

    public bool IncreaseWallTimer
    {
        get { return increaseWallTimer; }
        set { increaseWallTimer = value; }
    }

    public Vector2 WallAvoidDirection
    {
        get { return wallAvoidDirection; }
        set { wallAvoidDirection = value; }
    }
    //end of wall avoidance

    public float Restlessness
    {
        get { return restlessness; }
    }

    // DO NOT CHANGE THE role PARAMETER
    // change the Role -> this also changes the color
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
    Vector2 previousMove;
    public Vector2 PreviousMove { get { return previousMove; } }

    public bool showFOV;
    public bool manualMovement;

    Rigidbody2D agentRigidBody;

    public List<GameObject> allVisibleThings;
    public List<FlockAgent> visibleAgents;
    public List<FlockAgent> visibleBystanders;
    public List<FlockAgent> visibleProtesters;
    public List<FlockAgent> visibleLeaders;

    float leaderAttentionTimer;

    void ResetLeaderAttentionTimer()
    {
        leaderAttentionTimer = 0.5f;
    }

    void Start()
    {
        directionalPreference = (2*Random.Range(0,2)) - 1;
        ChangeHeadColor(Color.black);
        agentCollider = GetComponent<Collider2D>();
        desiredSpeed = 0f;
        CircleCollider2D colider = GetComponent<CircleCollider2D>();
        coliderRadius = colider.radius;
        SetAgentRole(role);
        agentRigidBody = GetComponent<Rigidbody2D>();
        restlessness = Random.Range(0f, restlessnessUpperbound);
    }

    public void CustomUpdate()
    {

        allVisibleThings = GroupContext.GetDistinctGameObjectFromHits(GroupContext.GetHits(GetVisibleAgents()));
        visibleAgents = GroupContext.GetFlockAgents(allVisibleThings);
        visibleBystanders = GroupContext.GetBystanders(visibleAgents);
        visibleProtesters = GroupContext.GetProtesters(visibleAgents);
        visibleLeaders = GroupContext.GetLeaders(visibleAgents);

        // leader agent logic here
        if (role == AgentRole.Leader)
            CalculateLeaderState();

        // methods specially for protesters and bystanders
        if (role == AgentRole.Protester || role == AgentRole.Bystander)
        {
            CalculateAgentState();
            CalculateContagion();
        }
    }


    public int GetLowestIndexOfVisibleAgents(List<FlockAgent> agentGroup)
    {
        if (agentGroup.Count() == 0)
            return -1;
        List<int> indexes = agentGroup.Where(obj => obj.leaderIndex != -1).Select(obj => obj.leaderIndex).ToList();
        if (indexes.Count() != 0)
            return indexes.Min();
        return -1;
    }
    public List<FlockAgent> GetListOfAgentsWithIndex(int index, List<FlockAgent> agentGroup)
    {
        return agentGroup.Where(obj => obj.leaderIndex == index).ToList();
    }

    public List<FlockAgent> GetListOfAgentsWithIndexHigherOrEqualThan(
        int index, List<FlockAgent> agentGroup)
    {
        return agentGroup.Where(agent => agent.leaderIndex >= index).ToList();
    }

    // calculates how the leader decides if he wants to be the leader
    void CalculateLeaderState()
    {
        if (!agentFlock.disableLeader)
        {
            // number of agents that have eye contact with leader
            int num = GetNumberOfAgentsWhoSeeMe();

            // if there are more than 7 people watching the leader the leader is highly motivated
            if (num > 4)
                ResetLeaderAttentionTimer();

            // if there are less than 5 people watching the leader the leader is losing his motivation
            if (num < 2)
                leaderAttentionTimer -= 0.04f * Time.deltaTime;

            // if there are less than 1 peopel watching hte leader the leader is losing motivation even faster
            if (num < 1)
                leaderAttentionTimer -= 0.07f * Time.deltaTime;

            // if the attention timer is less than zero the leader stops acting as a leader and becomes a protestor
            if (leaderAttentionTimer <= 0)
            {
                // he fees up his spot to let somebody else take over
                leaderAttentionTimer = 0;
                agentFlock.ResetLeaderIdentificationTimer();
                Role = AgentRole.Protester;
                manualMovement = false;
            }
        }
        else
        {
            leaderAttentionTimer = 0;
            agentFlock.ResetLeaderIdentificationTimer();
            Role = AgentRole.Protester;
            manualMovement = false;
        }
    }

    void CalculateAgentState()
    {

        // handle stationary
        if (state == AgentState.Stationary)
        {
            restlessness += 0.04f * Time.deltaTime;
            if (restlessness > restlessnessUpperbound)
            {

                state = AgentState.inMotion;
                desiredPosition = GenerateNewDesiredPosition();
                ResetRestlessness();
            }
        }

        // handle in motion
        if (state == AgentState.inMotion && OnDesiredPosition())
        {
            if (Role == AgentRole.Bystander)
                SetCustomBystanderRestlessnessUpperbound();
            desiredPosition = Vector3.zero;
            state = AgentState.Stationary;
        }

        // this means that a leader is present
        if (agentFlock.TimeToLeaderIdentification <= 0)
        {
            List<FlockAgent> leaderFollowers = GetListOfAgentsWithIndexHigherOrEqualThan(0, visibleAgents);
            if (visibleLeaders.Count() > 0)
            {
                State = AgentState.HerdMode;
                ChangeHeadColor(Color.yellow);
                leaderIndex = 0;
                leader = visibleLeaders.First();
            }
            else if (leaderFollowers.Count() > (visibleAgents.Count() - leaderFollowers.Count()) && leaderIndex == -1)
            {
                int lowestVisibleLeaderIndex = GetLowestIndexOfVisibleAgents(visibleAgents);
                State = AgentState.HerdMode;
                ChangeHeadColor(Color.yellow);
                leaderIndex = lowestVisibleLeaderIndex + 1;
                leader = GetListOfAgentsWithIndex(lowestVisibleLeaderIndex, visibleAgents).First().leader;
            }
            // TODO: MAYBE ADD A STOP CONDITION
        }
        else if (State == AgentState.HerdMode || State == AgentState.Flocking)
        {
            if (leaderIndex >= 0)
            {
                leader = null;
                State = AgentState.Flocking;
                herdCooldown = leaderIndex + Random.Range(0f, 1f);
                leaderIndex = -1;
            }
            if (herdCooldown <= 0)
            {
                ResetState();
            }
            else
            {
                herdCooldown -= 0.4f * Time.deltaTime;
            }
        }
    }

    void ResetState()
    {
        restlessness = Random.Range(0.5f, restlessnessUpperbound);
        ChangeHeadColor(Color.black);
        state = AgentState.Stationary;
        desiredPosition = Vector3.zero;
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
            defectionTimer = Random.Range(0, 0.5f);
            defectionProb = DefectionProbability(visibleProtesters.Count(), visibleBystanders.Count(), visibleLeaders.Count());
            if (Random.Range(0, 100f) < defectionProb * 100)
            {
                SetAgentRole(AgentRole.Bystander);
            }
        }
        if (recruitmentTimer > 1.0f)
        {
            recruitmentTimer = Random.Range(0, 0.5f);
            recruitmentProb = RecruitmentProbability(visibleProtesters.Count(), visibleBystanders.Count(), visibleLeaders.Count());
            if (Random.Range(0, 100f) < recruitmentProb * 100)
            {
                SetAgentRole(AgentRole.Protester);
            }
        }
    }

    float DefectionProbability(float protesterCount, float bystanderCount, float leaderCount)
    {
        //in case we see nothing
        if (bystanderCount == 0)
            return 0f;

        float motility = 100f; //mild unrest according to Clements
        float u2 = 0.5f;
        float someConst = 0.53f;
        //Debug.Log(motility/(motility+bystanderCount) * someConst);
        if (u2 >= motility / (motility + bystanderCount) * someConst)
        {
            return 0.2f; //to make defection build up with slowly with time, return less than 1
        }
        return 0f;
    }

    float RecruitmentProbability(float protesterCount, float bystanderCount, float leaderCount)
    {
        //in case we see nothing
        if (protesterCount == 0)
            return 0f;

        float motility = 100f; //mild unrest according to Clements
        float u1 = 0.5f;
        float someConst = 0.53f;
        //Debug.Log(motility/(motility+protesterCount) * someConst);
        if (u1 >= motility / (motility + protesterCount) * someConst)
        {
            return 0.2f; //to make recruitment build up with slowly with time, return less than 1
        }
        return 0f;
    }

    public void ResetRestlessness()
    {
        restlessness = Random.Range(0, 0.5f);
    }

    public void ChangeBodyColor(Color color)
    {
        ChangeSpiteColor(color, "Capsule");
    }

    public void ChangeHeadColor(Color color)
    {
        ChangeSpiteColor(color, "Circle");
    }

    private void ChangeSpiteColor(Color color, string name)
    {
        Transform body = transform.Find(name);
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
        Vector2 newVelocity = velocity;
        if (agentFlock.smoothMove)
        {
            float moveFactor = agentFlock.smoothMoveFactor * Time.deltaTime;
            Vector2 desiredDirectionChange = velocity - previousMove;
            Vector2 directionChange = desiredDirectionChange.normalized * moveFactor;
            if (desiredDirectionChange.magnitude > moveFactor)
                newVelocity = previousMove + directionChange;
        }
        agentRigidBody.linearVelocity = newVelocity;
        previousMove = newVelocity;
    }

    public List<(RaycastHit2D, Vector2)> GetVisibleAgents()
    {
        List<(RaycastHit2D, Vector2)> visibleObjects = new();
        Vector2 direction = transform.rotation * Vector2.up * agentFlock.eyesightDistance;
        float angle = 0f;
        while (angle < agentFlock.flockFOV)
        {
            if (angle == 0f)
            {
                visibleObjects.Add((Physics2D.Raycast(transform.position, direction, agentFlock.eyesightDistance), direction));
            }
            else
            {
                Vector2 positiveVector = Numbers.Rotate2D(direction, angle);
                Vector2 negativeVector = Numbers.Rotate2D(direction, -angle);
                visibleObjects.Add((Physics2D.Raycast(transform.position, positiveVector, agentFlock.eyesightDistance), positiveVector));
                visibleObjects.Add((Physics2D.Raycast(transform.position, negativeVector, agentFlock.eyesightDistance), negativeVector));
            }
            angle += agentFlock.visualAngleChange;
        }
        direction.Normalize();
        direction *= agentFlock.personalSpaceDistance;
        while (angle < 180)
        {
            Vector2 positiveVector = Numbers.Rotate2D(direction, angle);
            Vector2 negativeVector = Numbers.Rotate2D(direction, -angle);
            visibleObjects.Add((Physics2D.Raycast(transform.position, positiveVector, agentFlock.personalSpaceDistance), positiveVector));
            visibleObjects.Add((Physics2D.Raycast(transform.position, negativeVector, agentFlock.personalSpaceDistance), negativeVector));
            angle += agentFlock.personalSpaceAngleChange / 4;
        }
        return visibleObjects;
    }

    // note that the lookaround angle should be maximum 180 degrees 
    // the lookaround angle is in degrees
    // 180 -> 360
    public void LookAround(float lookAroundAngle)
    {
        List<RaycastHit2D> hits = new();
        Vector2 direction = transform.rotation * Vector2.up * agentFlock.eyesightDistance;
        float angle = 0f;
        while (angle < lookAroundAngle)
        {
            if (angle == 0f) { hits.Add(Physics2D.Raycast(transform.position, direction, agentFlock.eyesightDistance)); }
            else
            {
                Vector2 positiveVector = Numbers.Rotate2D(direction, angle);
                Vector2 negativeVector = Numbers.Rotate2D(direction, -angle);
                hits.Add(Physics2D.Raycast(transform.position, positiveVector, agentFlock.eyesightDistance));
                hits.Add(Physics2D.Raycast(transform.position, negativeVector, agentFlock.eyesightDistance));
            }
            angle += agentFlock.visualAngleChange;
        }
        visibleAgents = GroupContext.GetFlockAgents(GroupContext.GetDistinctGameObjectFromHits(hits));
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
                DesiredSpeed = Numbers.NextGaussian(6, 2);
                ResetLeaderAttentionTimer();
                ChangeBodyColor(Color.green);
                break;
            case AgentRole.Protester:
                DesiredSpeed = Numbers.NextGaussian(6, 2);
                restlessnessUpperbound = Random.Range(0.8f, 1f);
                ChangeBodyColor(Color.red);
                break;
            case AgentRole.Bystander:
                DesiredSpeed = Numbers.NextGaussian(3, 1f);
                SetCustomBystanderRestlessnessUpperbound();
                ChangeBodyColor(Color.white);
                break;
            case AgentRole.Police:
                ChangeBodyColor(Color.blue);
                break;
            default:
                throw new System.Exception("Agent does not have a role");
        }
    }

    private void SetCustomBystanderRestlessnessUpperbound()
    {
        LookAround(180);
        var calculatedRestlessness = 2 / (visibleProtesters.Count() + 1);
        restlessnessUpperbound = Random.Range(calculatedRestlessness, calculatedRestlessness + 0.2f);
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
                if (direction.magnitude > agentFlock.personalSpaceDistance + 0.1f)
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

    public int GetNumberOfAgentsWhoSeeMe()
    {
        int count = 0;
        LookAround(180);
        foreach (FlockAgent protester in visibleProtesters)
        {
            if (protester.visibleLeaders.Exists(agent => agent == this))
                count++;
        }
        return count;
    }

    // IDEA: here we can take the center (same calculation and everything)
    // of the flock protestors. So take in the entire flock 
    //
    //    (agents.Count(agent => agent.Role == AgentRole.Protester));
    //    (agents.Count(agent => agent.Role == AgentRole.Bystander));
    //    (agents.Count(agent => agent.Role == AgentRole.Police));
    //
    // the current implementation only takes into consideration the visible agents
    // on the positive side this is more realistic vision vise
    // on the other hand this is not more realistic if we take into consideration hearing
    // given the fact that the protestors are lourder we can easily pinpoint where the sound is coming from
    public Vector3 GenerateNewDesiredPosition()
    {
        LookAround(150);
        List<Vector3> agentPositions = visibleProtesters.Select(protester => protester.transform.position).ToList();
        if (agentPositions.Count() < 1)
        {
            float minDistance = Random.Range(5, 10f);
            float maxDistance = Random.Range(minDistance, 20f);
            return GenerateRandomPositionInsideRing(minDistance, maxDistance, transform.position);
        }

        Vector3 center = agentPositions.Aggregate(Vector3.zero, (curr, vec) => curr + vec) / agentPositions.Count();
        float distance = agentPositions.OrderByDescending(v => Vector3.Distance(v, center)).First().magnitude;
        if (Role == AgentRole.Protester)
        {
            return GenerateRandomPositionInsideRing(0f, distance, center);
        }
        else if (Role == AgentRole.Leader)
        {
            float minLeaderMoveDistance = 90;
            float maxLeaderMoveDistance = 140;
            return GenerateRandomPositionInsideRing(minLeaderMoveDistance, maxLeaderMoveDistance, transform.position);
        }
        else
        {
            float someConst = distance / 2;
            return GenerateRandomPositionInsideRing(distance, distance + someConst, center);
        }
    }

    // returns true if point (x,y) lies inside a building, false otherwise
    bool IsInsideBuilding(float x, float y)
    {
        List<GameObject> walls = GameObject.FindGameObjectsWithTag("map").ToList();
        foreach (GameObject wall in walls)
        {
            if (Vector3.Distance(new Vector3(x, y, 0), wall.transform.position) <= 0.1f)
                return true;
        }
        return false;
    }

    Vector3 GenerateRandomPositionInsideRing(float minDistance, float maxDistance, Vector3 center)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float randomDistance = Random.Range(minDistance, maxDistance);
        float x = center.x + randomDistance * Mathf.Cos(angle);
        float y = center.y + randomDistance * Mathf.Sin(angle);
        if (!IsInsideBuilding(x, y))
            return new(x, y, 0);
        else
            return GenerateRandomPositionInsideRing(minDistance, maxDistance, center);
    }

    public void DebugLogAgent0(object text)
    {
        if (name == "Agent 0")
            Debug.Log(text);
    }
}


