using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField] private List<ItemSO> allItems = new List<ItemSO>();
    private Dictionary<string, ItemSO> itemDict;

    private void OnEnable()
    {
        itemDict = new Dictionary<string, ItemSO>();
        foreach (var item in allItems)
        {
            if (!itemDict.ContainsKey(item.itemID))
                itemDict.Add(item.itemID, item);
        }
    }

    public ItemSO GetItemByID(string id)
    {
        if (itemDict != null && itemDict.TryGetValue(id, out ItemSO item))
            return item;

        Debug.LogWarning($"Item con ID {id} no encontrado.");
        return null;
    }
}


