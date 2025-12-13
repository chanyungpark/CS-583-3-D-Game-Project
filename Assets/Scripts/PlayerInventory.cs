using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Simple flag for now – later you can track multiple items.
    public bool HasExtinguisher { get; private set; }

    public void GiveExtinguisher()
    {
        HasExtinguisher = true;
        Debug.Log("PlayerInventory: Extinguisher acquired!");
    }
}