using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 4f;           // walking speed

    [Header("Jump / Gravity")]
    public float jumpHeight = 1.5f;        // how high the jump is
    public float gravity = -25f;           // stronger than -9.81 for CharacterController
    public float groundedGravity = -4f;    // small downward force when grounded

    private CharacterController controller;
    private Vector3 verticalVelocity;      // only Y is used

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleGravityAndJump();
    }

    private void HandleMovement()
    {
        // WASD + Arrow keys (Unity default axes)
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right
        float vertical   = Input.GetAxisRaw("Vertical");   // W/S or Up/Down

        // input in local space (x = strafe, z = forward/back)
        Vector3 inputDir = new Vector3(horizontal, 0f, vertical);

        if (inputDir.sqrMagnitude > 1f)
            inputDir.Normalize();

        // move relative to the player's facing direction
        // (player's yaw is controlled by your mouse-look script)
        Vector3 move = transform.TransformDirection(inputDir);

        controller.Move(move * moveSpeed * Time.deltaTime);
    }

    private void HandleGravityAndJump()
    {
        bool isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity.y < 0f)
        {
            // small downward force to keep you snapped to ground
            verticalVelocity.y = groundedGravity;
        }

        // Jump (Space by default in Unity: "Jump" input)
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // v = sqrt(h * -2 * g)
            verticalVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // apply gravity
        verticalVelocity.y += gravity * Time.deltaTime;

        // apply vertical motion
        controller.Move(verticalVelocity * Time.deltaTime);
    }
}