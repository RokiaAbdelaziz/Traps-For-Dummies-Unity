using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target; // our Player
    public float distance = 5.0f;
    public float minDistance = 1.0f; // Closest the camera can get
    public float sensitivity = 3.0f;
    public LayerMask collisionLayers; 

    private float mouseX, mouseY;
    private float currentDistance;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentDistance = distance;
    }

    void LateUpdate()
    {
        // 1. Mouse Input
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;
        mouseY = Mathf.Clamp(mouseY, -20, 60);

        Quaternion rotation = Quaternion.Euler(mouseY, mouseX, 0);

        // 2. Physics Check (Raycast)
        // We shoot a ray from the player toward where the camera wants to be
        Vector3 defaultPosition = target.position - (rotation * Vector3.forward * distance) + (Vector3.up * 1.5f);
        Vector3 direction = (defaultPosition - (target.position + Vector3.up * 1.5f)).normalized;

        RaycastHit hit;
        if (Physics.Raycast(target.position + Vector3.up * 1.5f, direction, out hit, distance, collisionLayers))
        {
            // If we hit a wall, set distance to the hit point (minus a small buffer)
            currentDistance = Mathf.Clamp(hit.distance * 0.9f, minDistance, distance);
        }
        else
        {
            // If no wall, stay at max distance
            currentDistance = distance;
        }

        // 3. Final Position
        Vector3 finalPosition = target.position - (rotation * Vector3.forward * currentDistance) + (Vector3.up * 1.5f);

        transform.rotation = rotation;
        transform.position = finalPosition;
    }
}