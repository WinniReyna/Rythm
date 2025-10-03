using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    public RawImage icon;
    public TMP_Text itemNameText;
    public TMP_Text descriptionText;
    public TMP_Text quantityText;

    private string itemID;
    private InventorySO inventorySO;
    private ItemDatabase itemDatabase;

    public void Setup(ItemSO itemSO, int quantity, InventorySO inventory, ItemDatabase db)
    {
        if (itemSO == null) return;

        itemID = itemSO.itemID;
        inventorySO = inventory;
        itemDatabase = db;

        icon.texture = itemSO.icon;
        itemNameText.text = itemSO.itemName;
        descriptionText.text = itemSO.description;
        quantityText.text = quantity.ToString();
    }

    public void OnUseButton()
    {
        Debug.Log($"Usaste {itemID}");
    }

    public void OnDeleteButton()
    {
        if (inventorySO != null)
        {
            inventorySO.RemoveItem(itemID, 1);
            InventoryManager.Instance.RefreshUI();
            FindObjectOfType<InventorySaveLoad>().SaveInventory();
        }
    }

    public void OnDropButton()
    {
        if (inventorySO == null || itemDatabase == null) return;

        ItemSO itemSO = itemDatabase.GetItemByID(itemID);
        if (itemSO != null && itemSO.prefab != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            Vector3 spawnPos = player != null ? player.transform.position + player.transform.forward : Vector3.zero;
            Instantiate(itemSO.prefab, spawnPos, Quaternion.identity);

            inventorySO.RemoveItem(itemID, 1);
            InventoryManager.Instance.RefreshUI();
            FindObjectOfType<InventorySaveLoad>().SaveInventory();
        }
    }
}




