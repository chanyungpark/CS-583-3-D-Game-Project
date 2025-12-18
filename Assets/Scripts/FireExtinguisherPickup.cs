using UnityEngine;

public class FireExtinguisherPickup : MonoBehaviour
{
    [Tooltip("Optional prompt shown when player is in range (e.g. 'Press E to pick up').")]
    public GameObject pickupPromptUI;

    [Header("References")]
    public GameObject extinguisherInHands;

    private bool pickedUp = false;
    private bool playerInRange;
    private PlayerInventory playerInventory;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip equipClip;


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
            PickUp();
        }
    }

    private void PickUp()
    {
        pickedUp = true;

        playerInventory.GiveExtinguisher();

        if(extinguisherInHands != null)
        {
            extinguisherInHands.SetActive(true);
        }

        ExtinguisherSpray spray = extinguisherInHands.GetComponent<ExtinguisherSpray>();

        if(spray != null)
        {
            spray.hasExtinguisher = true;
        }

        if (audioSource != null && equipClip != null)
        {
            audioSource.PlayOneShot(equipClip);
        }


        if (pickupPromptUI != null)
        {
            pickupPromptUI.SetActive(false);
        }

        Invoke(nameof(DisablePickup), 0.15f);


    }

    private void DisablePickup()
    {
        gameObject.SetActive(false);
    }

}