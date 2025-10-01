using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIItem : MonoBehaviour
{
    [Header("UI Elements")]
    public RawImage icon;
    public TMP_Text itemNameText;
    public TMP_Text descriptionText;
    public TMP_Text quantityText;

    [Header("Database Reference")]
    public ItemDatabase itemDatabase; // Asignar en Inspector

    private string itemID;
    private InventorySO inventorySO;

    public void Setup(ItemSO itemSO, int quantity, InventorySO inventory)
    {
        if (itemSO == null) return;

        itemID = itemSO.itemID;
        inventorySO = inventory;

        icon.texture = itemSO.icon;
        itemNameText.text = itemSO.itemName;
        descriptionText.text = itemSO.description;
        quantityText.text = quantity.ToString();
    }

    public void OnUseButton()
    {
        Debug.Log($"Usaste {itemID}");
        // Aquí va la lógica de usar el item
    }

    public void OnDeleteButton()
    {
        if (inventorySO != null)
        {
            inventorySO.RemoveItem(itemID, 1);
            InventoryManager.Instance.RefreshUI();
        }
    }

    public void OnDropButton()
    {
        if (inventorySO == null || itemDatabase == null) return;

        // Obtener el ItemSO desde la base de datos usando itemID
        ItemSO itemSO = itemDatabase.GetItemByID(itemID);
        if (itemSO != null && itemSO.prefab != null)
        {
            // Instanciar objeto frente al jugador
            GameObject player = GameObject.FindWithTag("Player");
            Vector3 spawnPos = player != null ? player.transform.position + player.transform.forward : Vector3.zero;
            Instantiate(itemSO.prefab, spawnPos, Quaternion.identity);

            // Quitar 1 del inventario
            inventorySO.RemoveItem(itemID, 1);
            InventoryManager.Instance.RefreshUI();

            Debug.Log($"Soltaste {itemID} en escena");
        }
    }
}



