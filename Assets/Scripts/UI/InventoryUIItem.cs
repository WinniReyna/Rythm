using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    [SerializeField] private RawImage icon;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text quantityText;

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
        if (itemDatabase == null) return;

        // Buscar el ScriptableObject del ítem
        ItemSO itemSO = itemDatabase.GetItemByID(itemID);
        if (itemSO == null)
        {
            Debug.LogWarning($"No se encontró el item con ID {itemID}");
            return;
        }

        // Verificar si implementa IUsableItem
        if (itemSO is IUsableItem usable)
        {
            var player = GameObject.FindWithTag("Player")?.GetComponent<PlayerInteraction>();
            if (player != null)
            {
                bool consumed = usable.Use(player);
                if (consumed)
                {
                    inventorySO.RemoveItem(itemID, 1);
                    InventoryManager.Instance.RefreshUI();
                    FindObjectOfType<InventorySaveLoad>().SaveInventory();
                }
            }
        }
        else
        {
            Debug.Log($"El ítem {itemID} no es usable.");
        }
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




