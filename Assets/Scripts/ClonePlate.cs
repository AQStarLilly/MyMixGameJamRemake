using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlate : MonoBehaviour
{
    public SpeechBubble linkedSpeechBubble;
    public bool activated = false;
    private Renderer rend;

    public Color usedColor = Color.grey;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated && other.CompareTag("Player"))
        {
            PlayerManager.Instance.CloneFromPlate(this);
            activated = true;

            if (linkedSpeechBubble != null)
            {
                linkedSpeechBubble.SetCloneActivated(); // Change the message
            }
        }

    }

    public void ActivatePlate()
    {
        activated = true;

        if (rend != null)
        {
            rend.material.color = usedColor;
        }
    }
}
