using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public GameObject speechBubbleUI;  // Assign in Inspector
    public Transform player;  // Assign the player or NPC this follows
    public float heightOffset = 2f;  // Adjust height above the player

    private void Start()
    {
        speechBubbleUI.SetActive(false);  // Hide at start
    }

    void Update()
    {
        if (speechBubbleUI.activeSelf)
        {
            // Ensure the speech bubble appears exactly above the NPC
            Vector3 targetPosition = transform.position + new Vector3(0, heightOffset, 0);
            speechBubbleUI.transform.position = targetPosition;

            // Make the speech bubble always face the camera
            speechBubbleUI.transform.LookAt(Camera.main.transform);
            speechBubbleUI.transform.Rotate(0, 180, 0); // Prevents flipping
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowSpeechBubble();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideSpeechBubble();
        }
    }

    private void ShowSpeechBubble()
    {
        speechBubbleUI.SetActive(true);
    }

    private void HideSpeechBubble()
    {
        speechBubbleUI.SetActive(false);
    }
}