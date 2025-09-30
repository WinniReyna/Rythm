using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/InventoryData")]
public class InventorySO : ScriptableObject
{
    [Header("Items guardados")]
    public List<InventoryItem> items = new List<InventoryItem>();

    /// <summary>Agrega un item al inventario</summary>
    public void AddItem(ItemSO itemSO, int quantity = 1)
    {
        if (itemSO == null) return;

        InventoryItem existing = items.Find(i => i.itemID == itemSO.itemID);
        if (existing != null)
        {
            existing.quantity += quantity;
        }
        else
        {
            items.Add(new InventoryItem(itemSO.itemID, quantity));
        }
    }

    /// <summary>Quita un item del inventario</summary>
    public void RemoveItem(ItemSO itemSO, int quantity = 1)
    {
        if (itemSO == null) return;

        InventoryItem existing = items.Find(i => i.itemID == itemSO.itemID);
        if (existing != null)
        {
            existing.quantity -= quantity;
            if (existing.quantity <= 0)
                items.Remove(existing);
        }
    }

    /// <summary>Serializa la lista de items a JSON</summary>
    public string Serialize()
    {
        return JsonUtility.ToJson(new InventorySaveData(items));
    }

    /// <summary>Deserializa JSON a la lista de items</summary>
    public void Deserialize(string json)
    {
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);
        items = data.items ?? new List<InventoryItem>();
    }

    [System.Serializable]
    private class InventorySaveData
    {
        public List<InventoryItem> items;

        public InventorySaveData(List<InventoryItem> items)
        {
            this.items = items;
        }
    }
}

