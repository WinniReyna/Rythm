using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // Prefab InventoryUIItem
    [SerializeField] private Transform contentPanel; // Panel con GridLayoutGroup

    public void RefreshUI(Inventory inventory)
    {
        // Limpiar items actuales
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        // Crear nuevos items
        foreach (var invItem in inventory.items)
        {
            // Cargar ItemSO desde Resources usando itemID
            ItemSO itemSO = Resources.Load<ItemSO>($"Items/{invItem.itemID}");
            if (itemSO != null)
            {
                GameObject itemGO = Instantiate(itemPrefab, contentPanel);
                InventoryUIItem itemUI = itemGO.GetComponent<InventoryUIItem>();
                itemUI.Setup(itemSO, invItem.quantity);
            }
            else
            {
                Debug.LogWarning("No se encontró ItemSO para: " + invItem.itemID);
            }
        }
    }
}

