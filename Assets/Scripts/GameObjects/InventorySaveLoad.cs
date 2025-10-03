using UnityEngine;
using System.IO;

public class InventorySaveLoad : MonoBehaviour
{
    [SerializeField] private InventorySO inventorySO;

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        LoadInventory();
    }

    public void SaveInventory()
    {
        InventoryData data = new InventoryData();
        foreach (var item in inventorySO.items)
        {
            data.items.Add(new InventoryItem(item.itemID, item.quantity));
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Inventario guardado en: " + savePath);
    }

    public void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventoryData data = JsonUtility.FromJson<InventoryData>(json);

            inventorySO.Clear();
            foreach (var item in data.items)
            {
                inventorySO.AddItem(item.itemID, item.quantity);
            }

            Debug.Log("Inventario cargado desde: " + savePath);
        }
        else
        {
            Debug.Log("No hay guardado previo, inventario vacío.");
        }
    }
}
