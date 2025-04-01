using UnityEngine;

public class TeleportPlate : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform destinationPlate; // Set this in the inspector to select the destination plate
    public float teleportOffset = 1f; 

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a Player or Clone
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            // Ensure we have a valid destination before teleporting
            if (destinationPlate != null)
            {
                Teleport(other.transform);
            }
            else
            {
                Debug.LogWarning("Destination plate not set for " + gameObject.name);
            }
        }
    }

    private void Teleport(Transform target)
    {
        Vector3 teleportPosition = destinationPlate.position + Vector3.up * teleportOffset;
        target.position = teleportPosition;

        Debug.Log(target.name + " teleported to " + destinationPlate.name);
    }
}