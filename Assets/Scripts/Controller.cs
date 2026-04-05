using UnityEngine; // Import the UnityEngine namespace to access Unity's core classes and functions

public class Controller : MonoBehaviour // This class handles player movement and looking around in a first person perspective
{
    public float walkSpeed = 6f; // Walking speed of the player
    public float lookSpeed = 2f; // Mouse sensitivity for looking around
    private CharacterController controller; // Reference
    private float xRotation = 0f; // Vertical rotation of the camera
    private Camera playerCam; // Reference to the player's camera

    void Start() // Initialisation
    {
        controller = GetComponent<CharacterController>(); // Get the CharacterController component attached to the player
        playerCam = GetComponentInChildren<Camera>(); // Get the Camera component from the player's child objects
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen for better control
    }

    void Update() // Called once per frame
    {
        HandleMovement(); // Handle player movement based on input
        HandleMouseLook(); // Handle player looking around based on mouse input
    }

    void HandleMovement() // Method to handle player movement
    {
        float h = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        float v = Input.GetAxisRaw("Vertical"); // Get vertical input
        Vector3 move = transform.right * h + transform.forward * v; // Calculate movement direction based on player orientation
        controller.Move(move.normalized * walkSpeed * Time.deltaTime); // Move the player
    }

    void HandleMouseLook() // Method to handle player looking around
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed; // Get horizontal mouse movement and apply sensitivity
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed; // Get vertical mouse movement and apply sensitivity
        transform.Rotate(Vector3.up * mouseX); // Rotate the player horizontally based on mouse movement
        xRotation -= mouseY; // Adjust vertical rotation based on mouse movement
        xRotation = Mathf.Clamp(xRotation, -85f, 85f); // Clamp vertical rotation to prevent flipping
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); // Rotate the camera vertically based on the adjusted rotation
    }
}