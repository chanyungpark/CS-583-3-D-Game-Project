using UnityEngine;

public class FireDamageZone : MonoBehaviour
{
    [Tooltip("Damage per second while the player is inside the fire.")]
    public float damagePerSecond = 15f;

    private void OnTriggerStay(Collider other)
    {
        // Try to find PlayerHealth on the object we touched or its parent
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health == null)
            health = other.GetComponentInParent<PlayerHealth>();

        if (health != null)
        {
            health.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}