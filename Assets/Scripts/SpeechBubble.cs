using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public GameObject speechBubbleUI;  // The entire speech bubble UI with TextMeshPro inside
    public Transform player;           // Assign the player or NPC this follows
    public float heightOffset = 2f;    // Adjust height above the player

    [Header("Speech Messages")]
    public string afterCloneMessage;   // Optional message to display after clone activation
    private bool cloneActivated = false;

    private TMP_Text speechText;
    private string initialMessage; // To store the default speech bubble text

    private void Start()
    {
        speechBubbleUI.SetActive(false);
        speechText = speechBubbleUI.GetComponentInChildren<TMP_Text>();

        if (speechText != null)
        {
            // Read the initial message from the TextMeshPro component
            initialMessage = speechText.text;
        }
        else
        {
            Debug.LogError("No TMP_Text component found in the speech bubble UI! Make sure your text uses TextMeshPro.");
        }
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
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            UpdateSpeechBubble();
            ShowSpeechBubble();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
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

    public void SetCloneActivated()
    {
        cloneActivated = true;
        UpdateSpeechBubble();
    }

    private void UpdateSpeechBubble()
    {
        if (speechText == null) return;

        // Display the appropriate message based on clone activation
        if (cloneActivated)
        {
            speechText.text = afterCloneMessage;
        }
        else
        {
            speechText.text = initialMessage;
        }
    }
}