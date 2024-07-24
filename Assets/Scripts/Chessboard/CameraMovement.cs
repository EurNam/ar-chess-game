using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // The target the camera will orbit around
    public float distance = 10.0f; // Distance from the target
    public float moveSpeed = 5.0f; // Speed of orbiting
    public float rotationSpeed = 100.0f; // Speed of rotating up and down

    private float currentAngle = 0.0f; // Current angle around the target
    private float currentVerticalAngle = 45.0f; // Initial vertical angle set to 45 degrees

    void Start()
    {
        // Set the initial position and rotation of the camera
        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentAngle, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target);
    }

    void Update()
    {
        // Handle horizontal movement (orbiting) with A and D keys
        if (Input.GetKey(KeyCode.D))
            currentAngle -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            currentAngle += moveSpeed * Time.deltaTime;

        // Handle vertical rotation with up and down arrow keys
        if (Input.GetKey(KeyCode.S))
            currentVerticalAngle -= rotationSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
            currentVerticalAngle += rotationSpeed * Time.deltaTime;

        // Clamp the vertical angle to prevent flipping
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, -90.0f, 90.0f);

        // Calculate the new position and rotation
        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentAngle, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target);
    }
}