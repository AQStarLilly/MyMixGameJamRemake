using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClonePlate : MonoBehaviour
{
    public SpeechBubble linkedSpeechBubble;
    private Renderer rend;

    public bool activated = false;
    public Color usedColor = Color.grey;

    private void Awake()
    {
        rend = GetComponent<Renderer>();

        // Subscribe to scene change event to reset plates
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            PlayerManager.Instance.CloneFromPlate(this);
            ActivatePlate();
        }
    }

    public void ActivatePlate()
    {
        activated = true;

        if (rend != null)
        {
            rend.material.color = usedColor;
        }

        if (linkedSpeechBubble != null)
        {
            linkedSpeechBubble.SetCloneActivated(); // Change the message
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetPlate();
    }

    private void ResetPlate()
    {
        activated = false;
    }
}