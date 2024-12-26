using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flock : MonoBehaviour {

    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new();

    [Range(10, 500)]
    public int protestorStartingCount = 250;

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

    public float agentFov = 60f;
    public float eyesightDistance = 20f;

    public FlockBehavior leaderBehavior;
    public FlockBehavior protesterBehavior;
    public FlockBehavior bystanderBehavior;


    void Start() 
    {
        // to avoid calculating squares every time 
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        // protesters
        for (int i = 0; i < protestorStartingCount; i++) 
        {
            CreateNewAgent(agentPrefab, agents, protestorStartingCount, "Protester " + i);
        }
    }

    void CreateNewAgent(FlockAgent prefab, List<FlockAgent> group, int startingCount, string name)
    {
        FlockAgent newAgent = InstantiateAgent(prefab, startingCount);
        newAgent.name = name;
        newAgent.tag = "agent";
        newAgent.Fov = agentFov;
        newAgent.EyesightDistance = eyesightDistance;
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
    }

    void MoveAllAgents(List<FlockAgent> agents)
    {
        foreach(FlockAgent agent in agents)
        {
            agent.Fov = agentFov;
            agent.EyesightDistance = eyesightDistance;
            MoveAgent(agent);
        }
    }

    void MoveAgent(FlockAgent agent)
    {
        // List<GameObject> nearby = GetNearbyObjects(agent);
        List<(RaycastHit2D, Vector2)> hits = agent.GetVisibleAgents();
        if (agent.showFOV) 
        {
            agent.DrawHits(hits);
        }
        List<GameObject> visibleAgents = hits.Where(pair => pair.Item1).Select((pair) => pair.Item1.collider.gameObject).ToList();
        Vector2 move = new();
        switch (agent.Role)
        {
            case AgentRole.Leader:
                move = leaderBehavior.CalculateMove(agent, visibleAgents, this); 
                break;
            case AgentRole.Protester:
                move = protesterBehavior.CalculateMove(agent, visibleAgents, this); 
                break;
            case AgentRole.Bystander:
                move = bystanderBehavior.CalculateMove(agent, visibleAgents, this); 
                break;
        }

        // TODO: limit maximum speed -- reimplement this
        // move*=driveFactor;
        // if (move.sqrMagnitude > squareMaxSpeed) 
        // {
        //     move = move.normalized * maxSpeed;
        // }

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
}
