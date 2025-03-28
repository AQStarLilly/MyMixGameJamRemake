using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("Player Setup")]
    public GameObject playerPrefab; 

    [Header("Clone Setup")]
    public Material cloneMaterial;
    public List<ClonePlate> clonePlates = new List<ClonePlate>();
    public List<GameObject> allPlayers = new List<GameObject>();

    private int currentPlayerIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameObject mainPlayer = GameObject.FindGameObjectWithTag("Player");
        allPlayers.Add(mainPlayer);

        ClonePlate[] plates = FindObjectsOfType<ClonePlate>();
        clonePlates.AddRange(plates);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && allPlayers.Count > 1)
        {
            SwitchToNextPlayer();
        }
    }

    public void CloneFromPlate(ClonePlate triggeringPlate)
    {
        foreach (ClonePlate plate in clonePlates)
        {
            if (!plate.activated && plate != triggeringPlate)
            {
                Vector3 spawnPos = plate.transform.position + Vector3.up * 0.7f;
                GameObject clone = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

                // Set clone layer
                clone.tag = "Clone";
                SetLayerRecursively(clone, LayerMask.NameToLayer("Clone"));

                Transform body = clone.transform.Find("Beak");
                if (body != null)
                {
                    Renderer rend = body.GetComponent<Renderer>();
                    if (rend != null && cloneMaterial != null)
                    {
                        rend.material = cloneMaterial;
                    }
                }

                // Disable clone input
                clone.GetComponent<PlayerController>().DisableInput();

                allPlayers.Add(clone);
                plate.ActivatePlate();
            }
        }
    }

    private void SwitchToNextPlayer()
    {
        allPlayers[currentPlayerIndex].GetComponent<PlayerController>().DisableInput();

        currentPlayerIndex = (currentPlayerIndex + 1) % allPlayers.Count;

        allPlayers[currentPlayerIndex].GetComponent<PlayerController>().EnableInput();
        Camera.main.GetComponent<PlayerCameraController>().target = allPlayers[currentPlayerIndex].transform;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
