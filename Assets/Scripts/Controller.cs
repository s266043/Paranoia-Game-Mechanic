using UnityEngine;
public class Controller : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float lookSpeed = 2f;
    private CharacterController controller;
    private float xRotation = 0f;
    private Camera playerCam;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCam = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }
    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        controller.Move(move.normalized * walkSpeed * Time.deltaTime);
    }
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}