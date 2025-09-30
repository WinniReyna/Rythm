using UnityEngine;
using System.IO;

public class InventorySaveLoad : MonoBehaviour
{
    [SerializeField] private InventorySO inventory;

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveInventory()
    {
        string json = inventory.Serialize();
        File.WriteAllText(savePath, json);
        Debug.Log("Inventario guardado en: " + savePath);
    }

    public void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            inventory.Deserialize(json);
            Debug.Log("Inventario cargado desde: " + savePath);
        }
    }
}
