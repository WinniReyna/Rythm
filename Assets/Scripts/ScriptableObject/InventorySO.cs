using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDataSO", menuName = "Inventory/InventorySO")]
public class InventorySO : ScriptableObject
{
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(string itemID, int amount)
    {
        InventoryItem existing = items.Find(i => i.itemID == itemID);
        if (existing != null)
            existing.quantity += amount;
        else
            items.Add(new InventoryItem(itemID, amount));
    }

    public void RemoveItem(string itemID, int amount)
    {
        InventoryItem existing = items.Find(i => i.itemID == itemID);
        if (existing != null)
        {
            existing.quantity -= amount;
            if (existing.quantity <= 0)
                items.Remove(existing);
        }
    }

    public void Clear()
    {
        items.Clear();
    }
}


