using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public Slider healthSlider;

    private void Start()
    {
        // Auto-wire things if not set in Inspector
        if (healthSlider == null)
            healthSlider = GetComponent<Slider>();

        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth != null && healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = playerHealth.maxHealth;
            healthSlider.value = playerHealth.currentHealth;
        }
    }

    private void Update()
    {
        if (playerHealth == null || healthSlider == null) return;

        healthSlider.value = playerHealth.currentHealth;
    }
}