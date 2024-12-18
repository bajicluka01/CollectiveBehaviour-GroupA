using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Flock : MonoBehaviour {

    // protestors
    public FlockAgent protesterPrefab;
    List<FlockAgent> protestors = new List<FlockAgent>();
    public FlockBehavior protesterBehavior;
    [Range(10, 500)]
    public int protesterStartingCount = 250;

    // leader
    public FlockAgent leaderPrefab;
    private FlockAgent leader;

    public FlockAgent Leader
    {
        get { return leader; } 
    }
    public FlockBehavior leaderBehavior;

    // bystanders
    public FlockAgent bystanderPrefab;
    List<FlockAgent> bystanders = new List<FlockAgent>();
    public FlockBehavior bystanderBehavior;
    [Range(10, 500)]
    public int bystanderStartingCount = 250;

    // the popo
    public FlockAgent policePrefab;
    List<FlockAgent> police = new List<FlockAgent>();
    public FlockBehavior policeBehavior;
    [Range(10, 500)]
    public int policeStartingCount = 250;

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

    public GameObject buildingPrefab;


    void Start() 
    {
        //to avoid square roots
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        // leader
        FlockAgent leaderAgent = InstantiateAgent(leaderPrefab, 0); 
        leaderAgent.name = "Zlatko";
        leaderAgent.tag = "agent";
        leaderAgent.Initialize(this);
        leader = leaderAgent;

        // protesters
        for (int i = 0; i < protesterStartingCount; i++) 
        {
            CreateNewAgent(protesterPrefab, protestors, protesterStartingCount, "Protester " + i);
        }

        // bystanders
        for (int i = 0; i < bystanderStartingCount; i++) 
        {
            CreateNewAgent(bystanderPrefab, bystanders, bystanderStartingCount, "Bystander " + i);
        } 

        // police
        for (int i = 0; i < policeStartingCount; i++) 
        {
            CreateNewAgent(policePrefab, police, policeStartingCount, "Police " + i);
        } 
    }

    void CreateNewAgent(FlockAgent prefab, List<FlockAgent> group, int startingCount, string name)
    {
        FlockAgent newAgent = InstantiateAgent(prefab, startingCount);
        newAgent.name = name;
        newAgent.tag = "agent";
        newAgent.Initialize(this);
        group.Add(newAgent);
    }

    FlockAgent InstantiateAgent(FlockAgent prefab, int startingCount)
    {
        //GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
        //obj.transform.SetParent(gameObject.transform, false);
        return Instantiate(
            prefab,
            UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
            Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
            //transform.position,
            //Quaternion.identity,
            transform
        );
    }

    // Update is called once per frame
    void Update() 
    {
        MoveAgent(leader, leaderBehavior);
        MoveAllAgents(protestors, protesterBehavior);
        MoveAllAgents(bystanders, bystanderBehavior);
        MoveAllAgents(police, policeBehavior);
    }

    void MoveAllAgents(List<FlockAgent> agents, FlockBehavior behavior)
    {
        foreach(FlockAgent agent in agents)
        {
            MoveAgent(agent, behavior);
        }
    }

    void MoveAgent(FlockAgent agent, FlockBehavior behavior)
    {
        List<Transform> context = GetNearbyObjects(agent);
        Vector2 move = behavior.CalculateMove(agent, context, this);
        // move*=driveFactor;
        // if (move.sqrMagnitude > squareMaxSpeed) 
        // {
        //     move = move.normalized * maxSpeed;
        // }
        agent.Move(move);
    }

    List<Transform> GetNearbyObjects(FlockAgent agent) 
    {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        //Collider2D[] contextColliders = Physics2D.OverlapPointAll(agent.transform.position);
        foreach(Collider2D c in contextColliders) 
        {
            if (c != agent.AgentCollider) 
            {
                context.Add(c.transform);
            }
        }
        //Debug.Log(contextColliders[0].gameObject.layer);
        return context;
    }

    // Call this function in order to change the group of a certain bystander or a protestor and pass the desired agent to be switched :)
    // TODO: test this code in order to see if it works
    public void SwitchProtesterBystanderSquad(FlockAgent agent) 
    {
        if (bystanders.Contains(agent))
        {
            bystanders.Remove(agent);
            FlockAgent newAgent = Instantiate(protesterPrefab, agent.transform.position, agent.transform.rotation, transform);
            protestors.Add(newAgent);
            Destroy(agent);
        } else if (protestors.Contains(agent))
        {
            protestors.Remove(agent);
            FlockAgent newAgent = Instantiate(bystanderPrefab, agent.transform.position, agent.transform.rotation, transform);
            bystanders.Add(newAgent);
            Destroy(agent);
        }
    }
}
