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
