using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClonePlate : MonoBehaviour
{
    public bool activated = false;
    private Renderer rend;

    public Color usedColor = Color.grey;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
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
    }
}
