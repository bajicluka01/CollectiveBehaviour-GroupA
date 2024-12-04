using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderFlock : MonoBehaviour {

    public LeaderAgent agentPrefab;
    List<LeaderAgent> agents = new List<LeaderAgent>();
    public LeaderBehavior behavior;

    [Range(1, 500)]
    public int startingCount = 300;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    public float driveFactor = 20f;
    [Range(1f, 100f)]
    public float maxSpeed = 50f;
    [Range(1f, 10f)]
    public float neighborRadius = 0.5f;
    [Range(0f, 1f)]
    public float avoidanceRadiusMultiplier = 0.5f;

    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //to avoid square roots
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++) {
            LeaderAgent newAgent = Instantiate(
                agentPrefab,
                UnityEngine.Random.insideUnitCircle * startingCount * AgentDensity,
                Quaternion.Euler(Vector3.forward * UnityEngine.Random.Range(0f, 360f)),
                transform
                );
            newAgent.name = "Agent "+i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void Update() {
        foreach(LeaderAgent agent in agents) {
            List<Transform> context = GetNearbyObjects(agent);
            //agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);

            Vector2 move = behavior.CalculateMove(agent, context, this);
            move*=driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed) {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    List<Transform> GetNearbyObjects(LeaderAgent agent) {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders) {
            if (c != agent.AgentCollider) {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
