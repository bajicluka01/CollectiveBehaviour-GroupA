using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;

public class SetupPhaseManager : MonoBehaviour
{
    public GameObject policePrefab; // The police agent GameObject
    private Camera mainCamera;
    private Vector2? initialMousePosition = null;
    private Vector2? finalMousePosition = null;
    private List<GameObject> activePoliceAgents = new List<GameObject>(); // List to track active police agents
    private const int maxPoliceCount = 50; // Maximum allowed police agents
    

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        // Detect the initial and final positions on mouse click
        if (Input.GetMouseButtonDown(0) && initialMousePosition == null)
        {
            initialMousePosition = DetectMousePressPosition();
        } else if (Input.GetMouseButtonUp(0) && initialMousePosition != null)
        {
            finalMousePosition = DetectMousePressPosition();

            // If both the initial and final mouse positions are detected, create a line of police agents
            if (finalMousePosition != null)
            {
                // Create a line of police agents between the initial and final mouse positions
                CreatePoliceAgentsBetweenPositions((Vector2)initialMousePosition, (Vector2)finalMousePosition);

                // Reset the initial and final mouse positions
                initialMousePosition = null;
                finalMousePosition = null;
            }
        }

        // Set police cound on textField
        TextFieldManager.setPolice(activePoliceAgents.Count);
    }

    private Vector2 DetectMousePressPosition()
    {
        // Convert the mouse position from screen space to world space
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

        // Return a 2D world position (ignore the z-coordinate)
        return new Vector2(worldPosition.x, worldPosition.y);
    }


    //TODO the user shouldn't be able to place police agents inside a building!
    private void CreatePoliceAgentsBetweenPositions(Vector2 initialPosition, Vector2 finalPosition)
    {
        // I want the number of agents to depend on the distance between the initial and final positions
        int numberOfAgents = Mathf.CeilToInt(Vector2.Distance(initialPosition, finalPosition) / 2);

        Debug.Log("[SetupPoliceManager] numberOfAgents: " + numberOfAgents);

        // Calculate the direction vector between the initial and final positions
        Vector2 direction = (finalPosition - initialPosition).normalized;

        // Calculate the distance between each police agent
        float distanceBetweenAgents = Vector2.Distance(initialPosition, finalPosition) / numberOfAgents;

        if (activePoliceAgents.Count + numberOfAgents > maxPoliceCount)
        {
            numberOfAgents = Math.Max(0, maxPoliceCount - activePoliceAgents.Count);
        }

        // Create a line of police agents between the initial and final positions
        for (int i = 0; i < numberOfAgents; i++)
        {
            // Calculate the position of the police agent
            Vector2 position = initialPosition + direction * i * distanceBetweenAgents;

            // Check if there is already an object in this position
            Collider2D existingObject = Physics2D.OverlapCircle(position, 0.5f); // Adjust radius as needed
            if (existingObject != null)
            {

                continue; // Skip this position if an object is already there
            }

            // Calculate the perpendicular direction (clockwise)
            Vector2 perpendicularDirection = new Vector2(direction.y, -direction.x);

            // Calculate the rotation from the perpendicular direction
            float angle = Mathf.Atan2(perpendicularDirection.y, perpendicularDirection.x) * Mathf.Rad2Deg + 90;
            Quaternion rotation = Quaternion.Euler(0, 0, angle); // Z-axis rotation for 2D

            // Instantiate the police agent at the calculated position
            GameObject policeAgent = Instantiate(policePrefab, position, rotation);

            // Change police body color to blue
            policeAgent.GetComponent<FlockAgent>().SetAgentRole(AgentRole.Police);

            // Add the police agent to the active list
            activePoliceAgents.Add(policeAgent);
        }
    }

    // private IEnumerator<WaitForSeconds> DestroyPoliceAgentAfterTime(GameObject policeAgent, float delay)
    // {
    //     yield return new WaitForSeconds(delay);

    //     // Remove from the active list and destroy the object
    //     activePoliceAgents.Remove(policeAgent);
    //     Destroy(policeAgent);
    // }

    public void ChangeBodyColor(Color color)
    {
        Transform body = transform.Find("Capsule");
        SpriteRenderer sr = body.GetComponent<SpriteRenderer>();
        sr.color = color;   
    }
}