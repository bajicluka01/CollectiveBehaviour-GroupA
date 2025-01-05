using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Flock : MonoBehaviour {

    // this timer decreases till it reached zero
    // once the timer reaches zero the leader is choosen
    // this process is then repeated
    float timeToLeaderIdentification;
    public float TimeToLeaderIdentification
    {
        get {return timeToLeaderIdentification;}
    }


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
    public FlockBehavior herdProtesterBehavior;
    
    public FlockBehavior stationaryBystanderBehavior;
    public FlockBehavior inMotionBystanderBehavior;
    public FlockBehavior herdBystanderBehavior;

    public bool manualLeaderMovement = false;
    [Range(0,3)]
    public float leaderIdentificationTimeInterval = 10f;

    public bool showUI;

    public bool disableLeader = false;

    void Start() 
    {
        // leader identificaiton initialization
        timeToLeaderIdentification = leaderIdentificationTimeInterval; 

        // to avoid calculating squares every time 
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < protestorStartingCount; i++) 
        {
            CreateNewAgent(agentPrefab, agents, protestorStartingCount, "Agent " + i);
        }
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

    void HandleLeaderSelection()
    {
        if (timeToLeaderIdentification > 0 && !disableLeader)
        {
            timeToLeaderIdentification -= 0.1f * Time.deltaTime;
            
            // this inner if is called only once when the time to leader identification reaches zero 
            if (timeToLeaderIdentification <= 0)
            {
                List<FlockAgent> protesters = agents.Where(agent => agent.Role == AgentRole.Protester).ToList();
                FlockAgent selectedLeader = SelectRandomAgent(protesters);  
                selectedLeader.Role = AgentRole.Leader;
                selectedLeader.manualMovement = manualLeaderMovement;
            }
        }
    }

    // resets the leader identification timer to a random float between timeInterval/2 and timeinterval
    // this method is called by the behaviour script or the agent in order to reset the leader selection process
    public void ResetLeaderIdentificationTimer()
    {
        timeToLeaderIdentification = UnityEngine.Random.Range(leaderIdentificationTimeInterval/2, leaderIdentificationTimeInterval);
    }

    FlockAgent SelectRandomAgent(List<FlockAgent> agentGroup)
    {
        return agentGroup.ElementAt(UnityEngine.Random.Range(0,agentGroup.Count()));
    }

    void Update() 
    {
        // Handle the leader selection process
        HandleLeaderSelection();
        
        // Move all agents
        MoveAllAgents(agents);

        // Change UI legend based on the number of agents
        if(showUI)
        {
            ChangeUILegend();
        }
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
            case (AgentRole.Protester, AgentState.HerdMode):
                move = herdProtesterBehavior.CalculateMove(agent, this); 
                break;

            case (AgentRole.Bystander, AgentState.inMotion):
                move = inMotionBystanderBehavior.CalculateMove(agent, this); 
                break;
            case (AgentRole.Bystander, AgentState.Stationary):
                move = stationaryBystanderBehavior.CalculateMove(agent, this); 
                break;
            case (AgentRole.Bystander, AgentState.HerdMode):
                move = herdBystanderBehavior.CalculateMove(agent, this); 
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
    }
}