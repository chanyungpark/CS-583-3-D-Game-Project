using UnityEngine;

public class FireDamageZone : MonoBehaviour
{
    [Tooltip("Damage per second while the player is inside the fire.")]
    public float damageToPlayer = 15f;
    public float damageToTree = 3f;

    private void OnTriggerStay(Collider other)
    {
        // Try to find PlayerHealth on the object we touched or its parent
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health == null)
            health = other.GetComponentInParent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damageToPlayer * Time.deltaTime);
        }

        // Try to find Tree Object
        TreeBurning tree = other.GetComponent<TreeBurning>();
        if(tree == null)
        {
            tree = other.GetComponentInParent<TreeBurning>();
        }
        if(tree != null)
        {
            tree.ApplyFireDamage(damageToTree * Time.deltaTime);
        }
    }
}