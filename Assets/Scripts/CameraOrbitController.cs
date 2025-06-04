using UnityEngine;
using UnityEngine.InputSystem;

public class CameraOrbitController : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // The object to orbit around

    [Header("Rotation")]
    public float rotationSpeed = 5f;
    public float minYAngle = 10f;
    public float maxYAngle = 80f;

    [Header("Zoom")]
    public float zoomSpeed = 5f;
    public float minDistance = 3f;
    public float maxDistance = 20f;
    public float initialDistance = 10f;

    private float currentYaw = 0f;
    private float currentPitch = 30f;
    private float currentDistance;

    void Start()
    {
        currentDistance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            currentYaw = angles.y;
            currentPitch = angles.x;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        var mouse = Mouse.current;
        if (mouse == null) return;

        // Rotation input (right mouse button)
        if (mouse.rightButton.isPressed)
        {
            Vector2 mouseDelta = mouse.delta.ReadValue();
            currentYaw += mouseDelta.x * rotationSpeed * 0.02f; // 0.02f to match old Input.GetAxis sensitivity
            currentPitch -= mouseDelta.y * rotationSpeed * 0.02f;
            currentPitch = Mathf.Clamp(currentPitch, minYAngle, maxYAngle);
        }

        // Zoom input (scroll wheel)
        float scroll = mouse.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            currentDistance -= scroll * zoomSpeed * 0.1f;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }

        // Calculate new position and rotation
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 direction = rotation * Vector3.back;
        Vector3 position = target.position + direction * currentDistance;

        transform.position = position;
        transform.LookAt(target.position);
    }
}