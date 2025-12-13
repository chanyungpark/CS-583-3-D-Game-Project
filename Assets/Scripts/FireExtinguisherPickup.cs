using UnityEngine;

public class FireExtinguisherPickup : MonoBehaviour
{
    [Tooltip("Optional prompt shown when player is in range (e.g. 'Press E to pick up').")]
    public GameObject pickupPromptUI;

    private bool playerInRange;
    private PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInventory = other.GetComponent<PlayerInventory>();
        if (playerInventory == null) return;

        playerInRange = true;
        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        if (pickupPromptUI != null)
            pickupPromptUI.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange || playerInventory == null) return;

        // E to pick up
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerInventory.GiveExtinguisher();
            FindObjectOfType<ExtinguisherSpray>().hasExtinguisher = true;
            if (pickupPromptUI != null)
                pickupPromptUI.SetActive(false);

            // Hide the extinguisher in the world
            gameObject.SetActive(false);
        }
    }
}