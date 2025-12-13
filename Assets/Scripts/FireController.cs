using UnityEngine;

public class FireController : MonoBehaviour
{
    [Header("Fire Health")]
    public float maxHealth = 100f;
    public bool destroyOnExtinguish = true;

    [Header("Visuals")]
    public ParticleSystem fireVFX;   // assign your fire particle here

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;

        // if not explicitly assigned, try to auto-grab a ParticleSystem on this object
        if (fireVFX == null)
            fireVFX = GetComponentInChildren<ParticleSystem>();
    }

    /// <summary>
    /// Called by ExtinguisherSpray – amount is “damage” from the spray.
    /// </summary>
    public void ApplyWater(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            Extinguish();
        }
    }

    private void Extinguish()
    {
        // stop the fire particles
        if (fireVFX != null)
        {
            var emission = fireVFX.emission;
            emission.enabled = false;
            fireVFX.Stop();
        }

        // optional: disable collider so it no longer hurts the player
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // optional: kill the whole fire object
        if (destroyOnExtinguish)
            Destroy(gameObject);
    }
}