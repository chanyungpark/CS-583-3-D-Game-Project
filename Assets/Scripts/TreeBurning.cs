using System;
using UnityEditor.Build.Content;
using UnityEngine;

public class TreeBurning : MonoBehaviour
{
    [Tooltip("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Tooltip("Burning")]
    public GameObject burningTreePrefab;

    private bool isBurned = false;


    private void Awake()
    {
        currentHealth = maxHealth;
    }


    public void ApplyFireDamage(float amount)
    {
        if(isBurned)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if(currentHealth <= 0f)
        {
            Burn();
        }
    }

    public void Burn()
    {
        if(isBurned)
            return;

        isBurned = true;

        if(burningTreePrefab != null)
        {
            Instantiate(burningTreePrefab, transform.position, transform.rotation);
        }

        if(GameManager.Instance != null)
        {
            GameManager.Instance.TreeBurned();
        }

        Destroy(gameObject);
    }

}
