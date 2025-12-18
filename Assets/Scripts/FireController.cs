using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("Fire Health")]
    public float maxHealth = 100f;
    public bool destroyOnExtinguish = true;

    [Header("Visuals")]
    public ParticleSystem fireVFX;

    private float currentHealth;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.MainModule main;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (fireVFX == null)
            fireVFX = GetComponentInChildren<ParticleSystem>();

        if (fireVFX != null)
        {
            emission = fireVFX.emission;
            main = fireVFX.main;
        }
    }

    public void ApplyWater(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateFireVisuals();

        if (currentHealth <= 0f)
        {
            Extinguish();
        }
    }

    private void UpdateFireVisuals()
    {
        if (fireVFX == null) return;

        float healthPercent = currentHealth / maxHealth;

        // Reduce emission as fire weakens
        emission.rateOverTime = Mathf.Lerp(0f, 50f, healthPercent);

        //reduce flame size
        main.startSize = Mathf.Lerp(0.2f, 1f, healthPercent);
    }

    private void Extinguish()
    {
        ScoreManager.Instance?.AddFireExtinguished();

        if (fireVFX != null)
        {
            fireVFX.Stop();
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        if (destroyOnExtinguish)
            Destroy(gameObject, 1f); 
    }
}