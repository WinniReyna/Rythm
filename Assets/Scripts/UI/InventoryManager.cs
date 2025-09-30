using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public Inventory inventory = new Inventory();

    [SerializeField] private InventoryUI inventoryUI; // Asignar en inspector

    private string savePath => Path.Combine(Application.persistentDataPath, "inventory.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
            LoadInventory(); // Cargar al inicio
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Abrir/cerrar inventario con I
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryUI != null)
            {
                bool isActive = inventoryUI.gameObject.activeSelf;
                inventoryUI.gameObject.SetActive(!isActive);

                if (!isActive)
                    inventoryUI.RefreshUI(inventory);
            }
        }
    }

    public void AddItem(string itemID, int quantity)
    {
        inventory.AddItem(itemID, quantity);
        if (inventoryUI != null)
            inventoryUI.RefreshUI(inventory);

        SaveInventory();
    }

    #region JSON Save/Load
    [System.Serializable]
    private class InventorySaveData
    {
        public string[] itemIDs;
        public int[] quantities;
    }

    public void SaveInventory()
    {
        InventorySaveData data = new InventorySaveData();
        int count = inventory.items.Count;
        data.itemIDs = new string[count];
        data.quantities = new int[count];

        for (int i = 0; i < count; i++)
        {
            data.itemIDs[i] = inventory.items[i].itemID;
            data.quantities[i] = inventory.items[i].quantity;
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadInventory()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        inventory.items.Clear();
        for (int i = 0; i < data.itemIDs.Length; i++)
        {
            inventory.AddItem(data.itemIDs[i], data.quantities[i]);
        }

        if (inventoryUI != null)
            inventoryUI.RefreshUI(inventory);
    }
    #endregion
}
