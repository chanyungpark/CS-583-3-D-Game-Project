using UnityEngine;

public class ExtinguisherSpray : MonoBehaviour
{
    [Header("Extinguisher Settings")]
    public bool hasExtinguisher = false;
    public float sprayStrength = 40f;
    public float maxDistance = 12f;

    [Header("Pressure Settings")]
    public float maxPressure = 100f;
    public float pressureDrainPerSecond = 20f;
    public float pressureRecoveryRate = 20f;

    [Header("References")]
    public ParticleSystem sprayVFX;
    public Transform nozzle;
    public LayerMask fireMask;

    private Camera _cam;
    private float _currentPressure;

    private void Start()
    {
        _cam = Camera.main;
        _currentPressure = maxPressure;

        if (sprayVFX != null)
            sprayVFX.Stop();
    }

    private void Update()
    {
        if (!hasExtinguisher)
        {
            StopSpray();
            return;
        }

        float dt = Time.deltaTime;

        HandleContinuousSpray(dt);
        HandlePressureRecovery(dt);
    }

    private void LateUpdate()
    {
        if (nozzle != null && _cam != null)
        {
            nozzle.rotation = Quaternion.LookRotation(_cam.transform.forward, Vector3.up);
        }
    }

    private void HandleContinuousSpray(float dt)
    {
        bool holdingSpray = Input.GetMouseButton(0);

        if (!holdingSpray || _currentPressure <= 0f)
        {
            StopSpray();
            return;
        }

        StartSpray();
        SprayRaycast(dt);

        _currentPressure -= pressureDrainPerSecond * dt;
        _currentPressure = Mathf.Clamp(_currentPressure, 0f, maxPressure);
    }

    private void SprayRaycast(float dt)
    {
        Ray ray;

        if (nozzle != null)
            ray = new Ray(nozzle.position, nozzle.forward);
        else
            ray = new Ray(_cam.transform.position, _cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, fireMask))
        {
            FireController fire = hit.collider.GetComponentInParent<FireController>();
            if (fire != null)
            {
                fire.ApplyWater(sprayStrength * dt);
            }
        }
    }

    private void StartSpray()
    {
        if (sprayVFX != null && !sprayVFX.isPlaying)
            Debug.Log("Spray VFX Play()");
            sprayVFX.Play();
    }

    private void StopSpray()
    {
        if (sprayVFX != null && sprayVFX.isPlaying)
            sprayVFX.Stop();
    }

    private void HandlePressureRecovery(float dt)
    {
        if (!Input.GetMouseButton(0))
        {
            _currentPressure += pressureRecoveryRate * dt;
            _currentPressure = Mathf.Clamp(_currentPressure, 0f, maxPressure);
        }
    }

    public float GetPressurePercent()
    {
        return maxPressure > 0f ? _currentPressure / maxPressure : 0f;
    }
}