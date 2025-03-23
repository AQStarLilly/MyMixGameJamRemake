using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerCameraController cameraController;


    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 7f;
    public float dashDuration = 0.2f;
    public float dashForce = 20f;
    public float dashCooldown = 1f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float groundCheckDistance = 1.1f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private bool isGrounded;
    private float lastDashTime;
    private bool isDashing = false;
    private Vector3 dashDirection;
    private float dashTimeRemaining;

    private bool inputEnabled = true;
    private Vector3 respawnPoint;

    [Header("Fall Death Settings")]
    public float fallDeathY = -10f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputActions = new PlayerInputActions();
        respawnPoint = transform.position; // start checkpoint is where player spawns

        inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Dash.performed += ctx => Dash();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    private void Update()
    {
        // Store input for use in FixedUpdate
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();

        // Ground check (basic raycast)
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (transform.position.y < fallDeathY)
        {
            Respawn();
        }
    }

    private void FixedUpdate()
    {
        if (!inputEnabled) return;

        Vector3 camForward = cameraController.GetCameraForwardFlat();
        Vector3 camRight = cameraController.GetCameraRightFlat();
        Vector3 move = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Apply better gravity
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.velocity.y > 0 && !inputActions.Player.Jump.IsPressed())
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Handle dashing separately
        if (isDashing)
        {
            rb.velocity = dashDirection * dashForce;
            dashTimeRemaining -= Time.fixedDeltaTime;
            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
            }
            return; // Skip regular movement during dash
        }

        // ===== Normal Movement & Rotation =====
        if (move.magnitude > 0.1f)
        {
            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }

        // These lines go here — only apply if NOT dashing
        Vector3 targetVelocity = move * moveSpeed;
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void DisableInput()
    {
        inputEnabled = false;
        rb.velocity = Vector3.zero;
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        respawnPoint = newCheckpoint;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void Dash()
    {
        if (Time.time - lastDashTime < dashCooldown)
            return;

        lastDashTime = Time.time;

        // Always dash forward from the player's current facing direction
        dashDirection = transform.forward;

        isDashing = true;
        dashTimeRemaining = dashDuration;

        // Optional: keep vertical velocity
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    private void Respawn()
    {
        rb.velocity = Vector3.zero; // reset movement
        transform.position = respawnPoint;
    }
}