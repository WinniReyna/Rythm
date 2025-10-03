using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject itemPrefab;
    public InventorySO inventorySO;
    public ItemDatabase itemDatabase;

    public void RefreshUI()
    {
        foreach (Transform child in itemsParent)
            Destroy(child.gameObject);

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





