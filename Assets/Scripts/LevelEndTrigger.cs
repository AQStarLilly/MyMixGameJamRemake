using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTrigger : MonoBehaviour
{
    public float delayBeforeLoad = 2f;
    public float liftSpeed = 1f;

    private bool isEnding = false;

    private void OnTriggerEnter(Collider other)
    {
        // Only allow the original player to trigger the goal
        if (!isEnding && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                isEnding = true;
                StartCoroutine(EndLevelSequence(player));
            }
        }
    }

    private IEnumerator EndLevelSequence(PlayerController player)
    {
        // Disable movement/input
        player.DisableInput();

        // Lift the player slowly for the duration
        float timer = 0f;
        while (timer < delayBeforeLoad)
        {
            player.transform.position += Vector3.up * liftSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Load next scene in build index
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.Log("End of game or no next scene set.");
        }
    }
}
