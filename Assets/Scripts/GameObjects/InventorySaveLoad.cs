using UnityEngine;
using System.IO;

public class InventorySaveLoad : MonoBehaviour
{
    [SerializeField] private InventorySO inventorySO;
    private ReturnPointHandler returnPointHandler;
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        LoadInventory();

        returnPointHandler = FindObjectOfType<ReturnPointHandler>();
        if (returnPointHandler == null)
            Debug.LogWarning("No se encontró ReturnPointHandler en la escena. El juego no se guardará al usar la puerta.");
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

        if (returnPointHandler != null)
        {
            returnPointHandler.SaveGameState();
            Debug.Log("Juego guardado automáticamente tras usar la puerta.");
        }
        
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
