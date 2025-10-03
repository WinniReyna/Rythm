using System.Collections.Generic;

[System.Serializable]
public class InventoryData
{
    public List<InventoryItem> items = new List<InventoryItem>();
}

[System.Serializable]
public class InventoryItem
{
    public string itemID;
    public int quantity;

    public InventoryItem(string id, int qty)
    {
        itemID = id;
        quantity = qty;
    }
}
