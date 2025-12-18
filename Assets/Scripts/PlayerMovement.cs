using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Walking speed in m/s.")]
    public float walkSpeed = 4f;
    [Tooltip("Sprint speed in m/s (Left Shift).")]
    public float sprintSpeed = 7f;
    [Tooltip("How quickly we accelerate/decelerate towards target speed.")]
    public float acceleration = 12f;

    [Header("Rotation")]
    [Tooltip("How quickly the character turns towards the move direction.")]
    public float rotationSmoothTime = 0.1f;

    [Header("Jump / Gravity")]
    [Tooltip("How high the player can jump (meters). Set to 0 to effectively disable jumping.")]
    public float jumpHeight = 1.5f;
    [Tooltip("Gravity strength. Keep negative.")]
    public float gravity = -25f;
    [Tooltip("Small downward force when grounded to keep us stuck to the ground.")]
    public float groundedGravity = -2f;

    [Header("Animator Settings")]
    [Tooltip("Multiplier for animator playback speed while sprinting.")]
    public float sprintAnimSpeedMultiplier = 1.2f;

    [Header("References")]
    [Tooltip("Camera the movement is relative to. If left empty, will use Camera.main.")]
    public Transform cameraTransform;
    [Tooltip("Animator on the player. If left empty, will auto-grab.")]
    public Animator animator;

    // Internal
    private CharacterController controller;

    // Velocity
    private Vector3 horizontalVelocity; // x/z
    private float verticalVelocity;     // y only

    // Smoothing
    private float currentSpeed;
    private float speedSmoothVelocity;
    private float turnSmoothVelocity;

    // State
    private bool isSprinting;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;

        float dt = Time.deltaTime;

        HandleMovement(dt);
        HandleGravityAndJump(dt);
        ApplyFinalMove(dt);
        UpdateAnimator(dt);
    }

    // ──────────────────────────────────────────────
    // Input + horizontal movement (no gravity here)
    // ──────────────────────────────────────────────
    private void HandleMovement(float dt)
    {
        // Input: WASD / arrow keys
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(inputX, inputZ);

        float inputMagnitude = Mathf.Clamp01(input.magnitude);
        Vector2 inputDir = inputMagnitude > 0.01f ? input.normalized : Vector2.zero;

        // Sprint only when actually moving
        isSprinting = Input.GetKey(KeyCode.LeftShift) && inputMagnitude > 0.01f;

        float targetSpeed = (isSprinting ? sprintSpeed : walkSpeed) * inputMagnitude;

        // Smooth speed change
        float smoothTime = targetSpeed > 0.1f ? (1f / acceleration) : 0.08f;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, smoothTime);

        if (inputDir == Vector2.zero)
        {
            // No input → gently slow to a stop
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, dt * acceleration);
            return;
        }

        // Build camera-relative forward/right on XZ plane
        Vector3 camForward = Vector3.forward;
        Vector3 camRight = Vector3.right;

        if (cameraTransform != null)
        {
            camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();
        }

        Vector3 moveDir;

        // 1) Pure backward: S only -> move backwards, don't rotate
        if (inputDir.y < -0.01f && Mathf.Abs(inputDir.x) < 0.01f)
        {
            moveDir = -transform.forward;
        }
        // 2) Pure strafe (A/D only, no forward component): move sideways, don't rotate
        else if (Mathf.Abs(inputDir.x) > 0.01f && inputDir.y <= 0.01f)
        {
            // Use camera right for strafe direction so A/D are camera-relative
            moveDir = (inputDir.x > 0f ? camRight : -camRight);
        }
        // 3) Forward or forward-diagonal: rotate character to face movement
        else
        {
            moveDir = camForward * inputDir.y + camRight * inputDir.x;
            moveDir.Normalize();

            float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                targetAngle,
                ref turnSmoothVelocity,
                rotationSmoothTime
            );
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        horizontalVelocity = moveDir * currentSpeed;
    }

    // ──────────────────────────────────────────────
    // Gravity & jumping, vertical motion only
    // ──────────────────────────────────────────────
    private bool IsGrounded()
    {
        return controller.isGrounded;
    }

    private void HandleGravityAndJump(float dt)
    {
        bool grounded = IsGrounded();

        if (grounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = groundedGravity;

            // Jump (Space) — only if jumpHeight > 0
            if (jumpHeight > 0f && Input.GetButtonDown("Jump"))
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            verticalVelocity += gravity * dt;
        }
    }

    // ──────────────────────────────────────────────
    // Combine horizontal + vertical and move once
    // ──────────────────────────────────────────────
    private void ApplyFinalMove(float dt)
    {
        Vector3 verticalMove = Vector3.up * verticalVelocity;
        Vector3 finalMove = horizontalVelocity + verticalMove;

        controller.Move(finalMove * dt);
    }

    // ──────────────────────────────────────────────
    // Animator hook
    // ──────────────────────────────────────────────
    private void UpdateAnimator(float dt)
    {
        if (animator == null) return;

        float normalizedSpeed = Mathf.InverseLerp(0f, sprintSpeed, currentSpeed);
        animator.SetFloat("Speed", normalizedSpeed, 0.1f, dt);

        animator.speed = isSprinting ? sprintAnimSpeedMultiplier : 1f;
    }
}