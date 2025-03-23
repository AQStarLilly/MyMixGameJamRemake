using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform respawnLocation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (respawnLocation != null)
                    player.SetCheckpoint(respawnLocation.position);
                else
                    player.SetCheckpoint(transform.position); // fallback
            }
        }
    }
}
