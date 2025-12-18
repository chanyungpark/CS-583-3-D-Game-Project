using UnityEngine;

public class ExtinguisherSpray : MonoBehaviour
{
    [Header("Extinguisher Settings")]
    [Tooltip("Set true when the player has picked up the extinguisher.")]
    public bool hasExtinguisher = false;

    [Tooltip("How long a single burst lasts (seconds).")]
    public float burstDuration = 0.2f;

    [Tooltip("Cooldown time between bursts (seconds).")]
    public float timeBetweenBursts = 0.3f;

    [Tooltip("How much suppression is applied per second while hitting a fire.")]
    public float sprayStrength = 40f;

    [Tooltip("Maximum range of the extinguisher raycast.")]
    public float maxDistance = 12f;

    [Header("Pressure Settings")]
    [Tooltip("Maximum extinguisher pressure.")]
    public float maxPressure = 100f;

    [Tooltip("Pressure consumed per burst.")]
    public float pressureDrainPerBurst = 25f;

    [Tooltip("Pressure recovered per second when not spraying.")]
    public float pressureRecoveryRate = 20f;

    [Header("References")]
    public ExtinguisherAudio extinguisherAudio;

    [Tooltip("Particle system for the extinguisher spray effect.")]
    public ParticleSystem sprayVFX;

    [Tooltip("Transform representing the nozzle (origin of raycast & VFX).")]
    public Transform nozzle;

    [Tooltip("LayerMask that identifies fire colliders.")]
    public LayerMask fireMask;

    // Internal state
    private Camera _cam;
    private float _currentPressure;
    private bool _isBursting;
    private float _burstTimer;
    private float _burstCooldownTimer;

    #region Unity Lifecycle

    private void Start()
    {
        _cam = Camera.main;
        _currentPressure = maxPressure;

        if (sprayVFX != null)
        {
            sprayVFX.Stop();
        }
    }

    private void Update()
    {
        // No extinguisher yet: no interaction
        if (!hasExtinguisher)
        {
            if (sprayVFX != null && sprayVFX.isPlaying)
            {
                sprayVFX.Stop();
            }
            if(extinguisherAudio != null)
            {
                extinguisherAudio.StopSpray();
            }
            return;
        }

        float dt = Time.deltaTime;

        HandlePressure(dt);
        HandleInput(dt);
        HandleBurst(dt);
    }

    private void LateUpdate()
    {
        // Force nozzle orientation to follow camera forward
        // so spray direction is independent of hand/idle rotations.
        if (nozzle != null && _cam != null)
        {
            nozzle.rotation = Quaternion.LookRotation(_cam.transform.forward, Vector3.up);
        }
    }

    #endregion

    #region Input / Burst Logic

    private void HandleInput(float dt)
    {
        // Cannot start a burst while cooling down or out of pressure
        bool canBurst = _burstCooldownTimer <= 0f && _currentPressure > 0f;

        if (Input.GetMouseButtonDown(0) && canBurst)
        {
            StartBurst();
        }
    }

    private void StartBurst()
    {
        _isBursting = true;
        _burstTimer = burstDuration;
        _burstCooldownTimer = timeBetweenBursts;

        _currentPressure -= pressureDrainPerBurst;
        _currentPressure = Mathf.Clamp(_currentPressure, 0f, maxPressure);

        if (sprayVFX != null && !sprayVFX.isPlaying)
        {
            sprayVFX.Play();
        }
        if(extinguisherAudio != null)
        {
            extinguisherAudio.StartSpray();
        }
    }

    private void HandleBurst(float dt)
    {
        // Handle cooldown when not bursting
        if (!_isBursting)
        {
            if (_burstCooldownTimer > 0f)
            {
                _burstCooldownTimer -= dt;
            }
            return;
        }

        // Active burst timer
        _burstTimer -= dt;
        if (_burstTimer <= 0f)
        {
            _isBursting = false;

            if (sprayVFX != null)
            {
                sprayVFX.Stop();
            }
            if (extinguisherAudio != null) 
            { 
                extinguisherAudio.StopSpray(); 
            }

            return;
        }

        // Raycast during active burst
        if (nozzle == null)
        {
            // Fallback to camera if nozzle is not assigned
            if (_cam == null) return;

            Ray fallbackRay = new Ray(_cam.transform.position, _cam.transform.forward);
            ProcessRaycast(fallbackRay, dt);
        }
        else
        {
            Ray ray = new Ray(nozzle.position, nozzle.forward);
            ProcessRaycast(ray, dt);
        }
    }

    private void ProcessRaycast(Ray ray, float dt)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, fireMask))
        {
            FireController fire = hit.collider.GetComponentInParent<FireController>();
            if (fire != null)
            {
                fire.ApplyWater(sprayStrength * dt);
            }
        }
    }

    #endregion

    #region Pressure

    private void HandlePressure(float dt)
    {
        if (!_isBursting)
        {
            _currentPressure += pressureRecoveryRate * dt;
            _currentPressure = Mathf.Clamp(_currentPressure, 0f, maxPressure);
        }
    }

    public float GetPressurePercent()
    {
        return maxPressure > 0f ? _currentPressure / maxPressure : 0f;
    }

    #endregion
}