using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    private List<InventoryItem> items = new List<InventoryItem>();

    private void AddItem(string itemID, int quantity)
    {
        InventoryItem existingItem = items.Find(i => i.itemID == itemID);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else
        {
            items.Add(new InventoryItem(itemID, quantity));
        }
    }
}

