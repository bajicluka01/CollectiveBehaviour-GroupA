using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flock : MonoBehaviour {

    public FlockAgent agentPrefab;
    readonly List<FlockAgent> agents = new();

    [Range(1, 500)]
    public int protestorStartingCount = 1;

    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 10f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(0.001f, 10f)]
    public float neighborRadius = 0.2f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    public float flockFOV = 60f;
    public float eyesightDistance = 20f;
    public float personalSpaceDistance = 1.7f;

    public FlockBehavior leaderBehavior;
    public FlockBehavior stationaryProtesterBehavior;
    public FlockBehavior inMotionProtesterBehavior;
    public FlockBehavior stationaryBystanderBehavior;
    public FlockBehavior inMotionBystanderBehavior;

    public bool manualLeaderMovement = false;

    void Start() 
    {
        // to avoid calculating squares every time 
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < protestorStartingCount; i++) 
        {
            CreateNewAgent(agentPrefab, agents, protestorStartingCount, "Agent " + i);
        }
        // TODO: remove this. this if only for the leader testing purposes -Nik
        //keep it, but add logic based on manualLeaderMovement -Luka
        FlockAgent lastAgent = agents.Last();
        lastAgent.Role = AgentRole.Leader;
        lastAgent.manualMovement = manualLeaderMovement;

        TextFieldManager.Initialize();
    }

    void CreateNewAgent(FlockAgent prefab, List<FlockAgent> group, int startingCount, string name)
    {
        FlockAgent newAgent = InstantiateAgent(prefab, startingCount);
        newAgent.name = name;
        newAgent.tag = "agent";
        newAgent.Fov = flockFOV;
        newAgent.EyesightDistance = eyesightDistance;
        newAgent.PeripsersonalDistance = personalSpaceDistance;
        newAgent.manualMovement = false; //this is only true for leader

        // an agent is randomly chosen to be either protester or bystander
        System.Random r = new System.Random();
        if (r.Next(100) < 50)
            newAgent.Role = AgentRole.Protester;
        else
            newAgent.Role = AgentRole.Bystander;
        newAgent.Initialize(this);
        group.Add(newAgent);
    }

    FlockAgent InstantiateAgent(FlockAgent prefab, int startingCount)
    {
        return Instantiate(
            prefab,
            UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
            Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
            transform
        );
    }

    void Update() 
    {
        MoveAllAgents(agents);

        // Change UI legend based on the number of agents
        ChangeUILegend();
    }

    void MoveAllAgents(List<FlockAgent> agents)
    {
        foreach(FlockAgent agent in agents)
        {
            // TODO: once we are done testing we can remove this interchangeable fov and set it
            // to initiate only on start -- we will debate if this will bring in enough of a speed up
            agent.Fov = flockFOV;
            agent.EyesightDistance = eyesightDistance;
            agent.PeripsersonalDistance = personalSpaceDistance;
            MoveAgent(agent);
        }
    }

    void MoveAgent(FlockAgent agent)
    {
        // first we update the internal state of the agent
        agent.CustomUpdate();

        // TODO: comment this out for performance
        List<(RaycastHit2D, Vector2)> hits = agent.GetVisibleAgents();
        if (agent.showFOV) 
        {
            agent.DrawHits(hits);
        }
        Vector2 move = new();
        switch (agent.Role, agent.State)
        {
            case (AgentRole.Leader, _):
                move = leaderBehavior.CalculateMove(agent, this); 
                agent.manualMovement = manualLeaderMovement; //maybe not the most elegant place for this, but I didn't wanna put an unnecessary if in MoveAllAgents
                break;
            case (AgentRole.Protester, AgentState.inMotion):
                move = inMotionProtesterBehavior.CalculateMove(agent, this); 
                break;
            case (AgentRole.Protester, AgentState.Stationary):
                move = stationaryProtesterBehavior.CalculateMove(agent, this); 
                break;
            case (AgentRole.Bystander, AgentState.inMotion):
                move = inMotionBystanderBehavior.CalculateMove(agent, this); 
                break;
            case (AgentRole.Bystander, AgentState.Stationary):
                move = stationaryBystanderBehavior.CalculateMove(agent, this); 
                break;
        }
        move*=driveFactor;
        if (move.sqrMagnitude > squareMaxSpeed) 
        {
            move = move.normalized * maxSpeed;
        }
        agent.Move(move);
    }

    List<GameObject> GetNearbyObjects(FlockAgent agent) 
    {
        List<GameObject> context = new();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders) 
        {
            if (c != agent.AgentCollider) 
            {
                context.Add(c.gameObject);
            }
        }
        return context;
    }

    void ChangeUILegend()
    {
        TextFieldManager.setProtestors(agents.Count(agent => agent.Role == AgentRole.Protester));
        TextFieldManager.setBystanders(agents.Count(agent => agent.Role == AgentRole.Bystander));
        TextFieldManager.setPolice(agents.Count(agent => agent.Role == AgentRole.Police));
    }
}