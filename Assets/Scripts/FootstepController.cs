using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("CharacterController used for movement.")]
    public CharacterController controller;

    [Tooltip("AudioSource used to play footstep sounds.")]
    public AudioSource footstepSource;

    [Header("Footstep Sounds")]
    [Tooltip("Footstep clips for natural / forest ground.")]
    public AudioClip[] forestSteps;

    [Header("Movement Settings")]
    [Tooltip("Minimum movement speed before footsteps start.")]
    public float moveThreshold = 0.1f;

    [Tooltip("Time between steps while walking.")]
    public float walkStepInterval = 0.45f;

    [Tooltip("Time between steps while sprinting (Shift).")]
    public float sprintStepInterval = 0.28f;

    [Header("Pitch")]
    public float walkPitch = 1.0f;
    public float sprintPitch = 1.15f;

    private float stepTimer = 0f;
    private bool wasMovingLastFrame = false;

    private void Update()
    {
        if (controller == null || footstepSource == null)
            return;

        // Are we moving enough to count as walking?
        bool isMoving = controller.isGrounded &&
                        controller.velocity.magnitude > moveThreshold;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) ||
                           Input.GetKey(KeyCode.RightShift);

        float currentInterval = isSprinting ? sprintStepInterval : walkStepInterval;
        float targetPitch     = isSprinting ? sprintPitch       : walkPitch;

        footstepSource.pitch = targetPitch;

        if (!isMoving)
        {
            // Not moving: reset and bail
            stepTimer = 0f;
            wasMovingLastFrame = false;
            return;
        }

        // Just started moving this frame → start with half interval
        if (!wasMovingLastFrame)
        {
            stepTimer = currentInterval * 0.5f; // first step sooner
        }

        wasMovingLastFrame = true;

        stepTimer += Time.deltaTime;

        if (stepTimer >= currentInterval)
        {
            PlayFootstep();
            stepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (forestSteps == null || forestSteps.Length == 0)
            return;

        AudioClip clip = forestSteps[Random.Range(0, forestSteps.Length)];
        footstepSource.PlayOneShot(clip);
    }
}