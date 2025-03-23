using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public Transform target;                 // The player
    public Vector3 offset = new Vector3(0, 3, -8);
    public float mouseSensitivity = 8f;
    public float followSpeed = 10f;

    private float yaw = 0f; // horizontal rotation
    private float pitch = 20f; // vertical rotation (optional clamp)
    public float minPitch = -10f;
    public float maxPitch = 60f;

    private void LateUpdate()
    {
        if (target == null) return;

        // Mouse drag to rotate
        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // Calculate rotation and apply offset
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smooth camera follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f); // Look at upper part of player
    }

    public Vector3 GetCameraForwardFlat()
    {
        Vector3 forward = transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    public Vector3 GetCameraRightFlat()
    {
        Vector3 right = transform.right;
        right.y = 0;
        return right.normalized;
    }
}
