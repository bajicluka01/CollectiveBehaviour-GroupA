using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SetupPhaseManager : MonoBehaviour
{
    public GameObject policePrefab; // The police agent GameObject
    private Camera mainCamera;
    private Vector2? initialMousePosition = null;
    private Vector2? finalMousePosition = null;
    private List<GameObject> activePoliceAgents = new List<GameObject>(); // List to track active police agents
    private List<PoliceLine> activePoliceLines = new List<PoliceLine>(); // List to track active police lines
    private const int maxPoliceCount = 50; // Maximum allowed police agents
    private bool draggingPolice = false;
    private int draggingPoliceIndex = -1;
    private PoliceLine? previousPoliceLine = null;

    struct PoliceLine
    {
        public Vector2 initialPosition;
        public Vector2 finalPosition;
        public float length;
        public float angle; // Angle of the line in degrees
        public List<GameObject> policeAgents; // List to track police agents in the line
    }

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
    }

    void Update()
    {
        HandleMouseInput();
        HandleDragging();
        HandleRotation();
        TextFieldManager.setPolice(activePoliceAgents.Count);
    }

    private void HandleMouseInput()
    {
        // Detect the initial position on mouse click
        if (Input.GetMouseButtonDown(0) && initialMousePosition == null)
        {
            initialMousePosition = DetectMousePressPosition();
            TryStartDragging();
        }
        // Detect mouse release
        else if (Input.GetMouseButtonUp(0) && initialMousePosition != null)
        {
            finalMousePosition = DetectMousePressPosition();
            HandleMouseRelease();
        }
    }

    private void TryStartDragging()
    {
        for (int i = 0; i < activePoliceLines.Count; i++)
        {
            if (IsPointOnLine(initialMousePosition.Value, activePoliceLines[i].initialPosition, activePoliceLines[i].finalPosition))
            {
                draggingPolice = true;
                draggingPoliceIndex = i;
                previousPoliceLine = activePoliceLines[i];
                Debug.Log("Started dragging police line.");
                break;
            }
        }
    }

    private void HandleMouseRelease()
    {
        if (draggingPolice)
        {
            var line = activePoliceLines[draggingPoliceIndex];
            if (IsLineOccupied(line))
            {
                Debug.Log("Cannot release line here: Another object is in the way!");
                RevertToPreviousLine();
            }
            else
            {
                // Update agent positions to match the new line state
                UpdatePoliceAgentsPositions(line);
            }
            ResetDragState();
        }
        else if (finalMousePosition.HasValue)
        {
            CreateNewPoliceLine(new PoliceLine
            {
                initialPosition = initialMousePosition.Value,
                finalPosition = finalMousePosition.Value,
            });
        }
        ResetMousePositions();
    }

    private void ResetDragState()
    {
        draggingPolice = false;
        draggingPoliceIndex = -1;
        previousPoliceLine = null;
    }

    private void ResetMousePositions()
    {
        initialMousePosition = null;
        finalMousePosition = null;
    }

    private void HandleDragging()
    {
        if (!draggingPolice) return;

        var currentMousePosition = DetectMousePressPosition();
        var tempLine = activePoliceLines[draggingPoliceIndex];
        Vector2 offset = currentMousePosition - (tempLine.initialPosition + tempLine.finalPosition) / 2;
        tempLine.initialPosition += offset;
        tempLine.finalPosition += offset;
        activePoliceLines[draggingPoliceIndex] = tempLine;
        UpdatePoliceAgentsPositions(tempLine);
    }

    private void HandleRotation()
    {
        if (!draggingPolice || !Input.GetKey(KeyCode.R)) return;

        float rotationIncrement = 0.5f; // Degrees per frame

        var tempLine = activePoliceLines[draggingPoliceIndex];
        Vector2 center = (tempLine.initialPosition + tempLine.finalPosition) / 2;
        Vector2 direction = tempLine.finalPosition - tempLine.initialPosition;

        Quaternion rotation = Quaternion.Euler(0, 0, rotationIncrement);
        direction = rotation * direction;

        tempLine.initialPosition = center - direction / 2;
        tempLine.finalPosition = center + direction / 2;
        tempLine.angle += rotationIncrement;

        activePoliceLines[draggingPoliceIndex] = tempLine;
        UpdatePoliceAgentsPositions(tempLine);
    }

    private void RevertToPreviousLine()
    {
        if (!previousPoliceLine.HasValue) return;
        activePoliceLines[draggingPoliceIndex] = previousPoliceLine.Value;
        UpdatePoliceAgentsPositions(previousPoliceLine.Value);
    }

    private void CreateNewPoliceLine(PoliceLine line)
    {
        Vector2 initial = line.initialPosition;
        Vector2 final = line.finalPosition;

        if (IsLineOccupied(line))
        {
            Debug.Log("Cannot place line here: Another object is in the way!");
            return;
        }

        float angle = CalculateLineAngle(initial, final);
        var policeAgents = CreatePoliceAgentsBetweenPositions(initial, final);
        activePoliceLines.Add(new PoliceLine
        {
            initialPosition = initial,
            finalPosition = final,
            length = Vector2.Distance(initial, final),
            angle = angle,
            policeAgents = policeAgents
        });
    }

    private Vector2 DetectMousePressPosition()
    {
        Vector3 screenPosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        return new Vector2(worldPosition.x, worldPosition.y);
    }

    private List<GameObject> CreatePoliceAgentsBetweenPositions(Vector2 initial, Vector2 final)
    {
        var agents = new List<GameObject>();
        float lineLength = Vector2.Distance(initial, final);
        int agentCount = Mathf.Clamp(Mathf.CeilToInt(lineLength / 2), 0, maxPoliceCount - activePoliceAgents.Count);
        Vector2 direction = (final - initial).normalized;
        float spacing = lineLength / Mathf.Max(agentCount, 1);

        for (int i = 0; i < agentCount; i++)
        {
            Vector2 position = initial + direction * i * spacing;
            if (Physics2D.OverlapCircle(position, 0.5f) != null) continue;
            Quaternion rotation = Quaternion.Euler(0, 0, CalculateLineAngle(initial, final));
            var agent = Instantiate(policePrefab, position, rotation);
            agent.GetComponent<FlockAgent>().SetAgentRole(AgentRole.Police);
            agents.Add(agent);
            activePoliceAgents.Add(agent);
        }
        return agents;
    }

    private bool IsPointOnLine(Vector2 point, Vector2 lineStart, Vector2 lineEnd)
    {
        float lineLength = Vector2.Distance(lineStart, lineEnd);
        float distToStart = Vector2.Distance(point, lineStart);
        float distToEnd = Vector2.Distance(point, lineEnd);
        return Mathf.Abs((distToStart + distToEnd) - lineLength) < 0.1f;
    }

    private bool IsLineOccupied(PoliceLine line)
    {
        Vector2 initial = line.initialPosition;
        Vector2 final = line.finalPosition;
        List<GameObject> lineAgents = line.policeAgents;

        // Define the resolution and thickness of the checks
        float resolution = 0.1f; // Spacing between checks
        float radius = 0.1f; // Collision radius for each check point

        // Calculate direction and distance
        Vector2 direction = (final - initial).normalized;
        float length = Vector2.Distance(initial, final);

        // Debug the line being checked
        Debug.DrawLine(initial, final, Color.red, 2f);

        // Perform checks along the line
        for (float distance = 0; distance <= length; distance += resolution)
        {
            Vector2 checkPosition = initial + direction * distance;

            // Check for objects at this position
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(checkPosition, radius);

            foreach (Collider2D collider in hitObjects)
            {
                if (collider == null) continue;

                // Exclude police agents belonging to the current line
                GameObject obj = collider.gameObject;
                if (lineAgents.Contains(obj)) continue;

                // Detect other objects
                if (!obj.CompareTag("Police")) // Ignore police agents from other lines
                {
                    Debug.Log($"Collision detected with object: {obj.name} at {checkPosition}");
                    return true;
                }
            }
        }

        return false;
    }

    private void UpdatePoliceAgentsPositions(PoliceLine line)
    {
        Vector2 direction = (line.finalPosition - line.initialPosition).normalized;
        float spacing = line.length / Mathf.Max(line.policeAgents.Count - 1, 1);

        for (int i = 0; i < line.policeAgents.Count; i++)
        {
            var position = line.initialPosition + direction * i * spacing;
            line.policeAgents[i].transform.position = position;
            line.policeAgents[i].transform.rotation = Quaternion.Euler(0, 0, line.angle);
        }
    }

    float CalculateLineAngle(Vector2 point1, Vector2 point2)
    {
        float deltaX = point2.x - point1.x;
        float deltaY = point2.y - point1.y;
        float angle = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg; // Convert radians to degrees
        return angle;
    }
}
