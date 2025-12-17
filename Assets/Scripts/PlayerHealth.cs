using UnityEngine;
using UnityEngine.SceneManagement; // only if you want a quick reload on death

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        Debug.Log($"Player took {amount:F1} damage. HP: {currentHealth:F1}/{maxHealth}");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player died!");

        // TEMP: reload scene on death
        // Comment this out later and hook to proper game over UI.
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDied();
        }
    }
}