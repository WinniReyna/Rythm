using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject inventoryPanel;
    public InventoryUI inventoryUI;
    public InventorySO inventorySO;

    private IInputProvider inputProvider;

    private bool isOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        inputProvider = new KeyboardInputProvider();
    }

    private void Update()
    {
        if (inputProvider.InventoryPanel())
            ToggleInventory();
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        if (inventoryPanel != null)
            inventoryPanel.SetActive(isOpen);

        if (isOpen)
            RefreshUI();
    }

    public void RefreshUI()
    {
        if (inventoryUI != null)
            inventoryUI.RefreshUI();
    }

    public void AddItem(string itemID, int amount = 1)
    {
        inventorySO.AddItem(itemID, amount);
        RefreshUI();
    }

    public void RemoveItem(string itemID, int amount = 1)
    {
        inventorySO.RemoveItem(itemID, amount);
        RefreshUI();
    }
}

