using UnityEngine;

public class InventoryUI : MonoBehaviour, IMenuPanel
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private InventorySO inventorySO;
    [SerializeField] private ItemDatabase itemDatabase;

    public void Open()
    {
        gameObject.SetActive(true);
        RefreshUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void RefreshUI()
    {
        if (itemsParent == null || inventorySO == null || itemDatabase == null) return;

        // Limpiar items antiguos
        foreach (Transform child in itemsParent)
            Destroy(child.gameObject);

        // Crear UI de cada item
        foreach (var invItem in inventorySO.items)
        {
            ItemSO itemSO = itemDatabase.GetItemByID(invItem.itemID);
            if (itemSO == null) continue;

            GameObject go = Instantiate(itemPrefab, itemsParent);
            InventoryUIItem uiItem = go.GetComponent<InventoryUIItem>();
            uiItem.Setup(itemSO, invItem.quantity, inventorySO, itemDatabase);
        }
    }
}




