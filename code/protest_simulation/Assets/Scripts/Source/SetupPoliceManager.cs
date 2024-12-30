using UnityEngine;

public class SetupPhaseManager : MonoBehaviour
{
    /*public GameObject policeAgent; // The police agent GameObject
    public KeyCode startKey = KeyCode.Return; // Key to start the animation
    private bool isSetupPhase = true; // Flag for the setup phase
    private Camera mainCamera;

    void Start()
    {
        // mainCamera = Camera.main;

        // Disable the police agent's animation initially
        // var animator = policeAgent.GetComponent<Animator>();
        // if (animator != null)
        // {
        //     animator.enabled = false; // Disable animations during setup
        // }
    }

    void Update()
    {
        // Debug.Log("[SetupPoliceManager.cs] isSetupPhase: " + isSetupPhase);
        // if (isSetupPhase)
        // {
        //     // Allow the user to drag and position the police agent with the mouse
        //     HandleMouseDragging();

        //     // Check if the user presses the start key to finish setup
        //     if (Input.GetKeyDown(startKey))
        //     {
        //         StartSimulation();
        //     }
        // }
    }

    private void HandleMouseDragging()
    {
        // Debug.Log("[SetupPoliceManager.cs] mainCamera: " + mainCamera);
        // Debug.Log("[SetupPoliceManager.cs] Input.mousePosition: " + Input.mousePosition);
        // Get the mouse position in world space
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Move the police agent to the mouse pointer position
            policeAgent.transform.position = hit.point;
        }
    }

    private void StartSimulation()
    {
        isSetupPhase = false; // End the setup phase

        // Enable the police agent's animation
        var animator = policeAgent.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true; // Enable the animator
            animator.SetTrigger("StartAnimation"); // Trigger the start animation
        }

        Debug.Log("Simulation Started! Police agent placed at position: " + policeAgent.transform.position);
    }*/
}
