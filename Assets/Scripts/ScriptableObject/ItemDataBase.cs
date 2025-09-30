using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDataBase : ScriptableObject
{
    public ItemSO[] items;

    public ItemSO GetItemByID(string id)
    {
        foreach (var item in items)
        {
            if (item.itemID == id) return item;
        }
        return null;
    }
}
