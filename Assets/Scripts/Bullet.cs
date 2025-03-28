using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask hitLayer;
    public float bulletSpeed = 10f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    private void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Bullet"); // Ensure bullet is on correct layer
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            rb.isKinematic = false; //  Ensure bullets are not kinematic when reactivated
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Find the root player component, even if a child object is hit
        PlayerController player = other.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            player.Respawn(); // Reset the player to checkpoint
            DeactivateBullet();
            return;
        }

        // Check if the bullet hit an object on the hitLayer (e.g., walls)
        if (((1 << other.gameObject.layer) & hitLayer) != 0)
        {
            Debug.Log("Bullet hit wall: " + other.gameObject.name);
            DeactivateBullet();
            return;
        }
    }

    //  Function to properly deactivate the bullet
    private void DeactivateBullet()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero; //  Stop movement
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true; //  Prevents further physics interactions
        }

        gameObject.SetActive(false); //  Disable before returning to pool
        BulletPool.Instance.ReturnBullet(gameObject);
    }
}
