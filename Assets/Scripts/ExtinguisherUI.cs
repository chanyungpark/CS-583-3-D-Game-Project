using UnityEngine;
using UnityEngine.UI;

public class ExtinguisherUI : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;    // Drag your Firefighter here
    public Image extinguisherIcon;            // The little icon in your UI
    public GameObject acquiredPopup;          // Optional "Extinguisher acquired!" popup

    private bool lastHasExtinguisher = false;

    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
        }
    }

    private void Start()
    {
        if (extinguisherIcon != null)
            extinguisherIcon.enabled = false;

        if (acquiredPopup != null)
            acquiredPopup.SetActive(false);

        RefreshUI();
    }

    private void Update()
    {
        if (playerInventory == null) return;

        if (playerInventory.HasExtinguisher != lastHasExtinguisher)
        {
            lastHasExtinguisher = playerInventory.HasExtinguisher;
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (playerInventory == null) return;

        bool hasExt = playerInventory.HasExtinguisher;

        if (extinguisherIcon != null)
            extinguisherIcon.enabled = hasExt;

        if (hasExt && acquiredPopup != null)
        {
            acquiredPopup.SetActive(true);
            CancelInvoke(nameof(HidePopup));
            Invoke(nameof(HidePopup), 2f);
        }
    }

    private void HidePopup()
    {
        if (acquiredPopup != null)
            acquiredPopup.SetActive(false);
    }
}