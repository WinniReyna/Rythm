using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private GameObject inventoryPanel; 
    [SerializeField] private InventoryUI inventoryUI; 
    [SerializeField] private InventorySO inventorySO;
    private InventorySaveLoad saveLoad;

    private IInputProvider inputProvider;

    public InventorySO InventorySO => inventorySO;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        saveLoad = FindObjectOfType<InventorySaveLoad>();
        inputProvider = new KeyboardInputProvider();
    }

    private void Update()
    {
        if (inputProvider.InventoryPanel())
        {
            if (PauseManager.Instance != null && inventoryPanel != null)
            {
                // Si el panel ya es currentPanel, ciérralo
                if (PauseManager.Instance.CurrentPanel == inventoryPanel.GetComponent<IMenuPanel>()) PauseManager.Instance.CloseCurrentPanel();
                else PauseManager.Instance.OpenPanel(inventoryPanel);
                
            }
        }
    }
    private void OpenInventory()
    {
        if (inventoryPanel == null)
        {
            Debug.LogWarning("InventoryPanel no asignado!");
            return;
        }

        if (PauseManager.Instance == null)
        {
            Debug.LogWarning("PauseManager.Instance es null!");
            return;
        }

        PauseManager.Instance.OpenPanel(inventoryPanel);

        // Actualizamos la UI después de abrirlo
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (inventoryUI != null)
            inventoryUI.RefreshUI();
    }

    public bool HasItem(string itemID, int requiredAmount = 1)
    {
        if (inventorySO == null || inventorySO.items == null)
            return false;

        var item = inventorySO.items.Find(i => i.itemID == itemID);
        return item != null && item.quantity >= requiredAmount;
    }

    public void AddItem(string itemID, int amount = 1)
    {
        inventorySO.AddItem(itemID, amount);
        RefreshUI();
        saveLoad.SaveInventory();
    }

    public void RemoveItem(string itemID, int amount = 1)
    {
        inventorySO.RemoveItem(itemID, amount);
        RefreshUI();
        saveLoad.SaveInventory();
    }
}


