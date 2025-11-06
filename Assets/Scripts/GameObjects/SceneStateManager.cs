using UnityEngine;
using System.Collections.Generic;

public class SceneStateManager : MonoBehaviour
{
    [Header("Objetos a controlar")]
    [SerializeField] private List<GameObject> objectsToManage; // objetos normales
    [SerializeField] private List<Teleporter> doorsToManage;   // puertas separadas

    private const string ActiveKey = "ActiveObject";

    // Guardar el estado
    public void SaveState()
    {
        // Objetos normales
        GameObject activeObj = null;
        foreach (var obj in objectsToManage)
        {
            if (obj.activeSelf)
            {
                activeObj = obj;
                break;
            }
        }
        if (activeObj != null)
            PlayerPrefs.SetString(ActiveKey, activeObj.name);

        // Puertas
        foreach (var door in doorsToManage)
            door.SaveState();

        PlayerPrefs.Save();
        Debug.Log("Estado guardado (objetos normales + puertas).");
    }

    // Cargar estado
    public void LoadState()
    {
        // Objetos normales
        if (PlayerPrefs.HasKey(ActiveKey))
        {
            string activeName = PlayerPrefs.GetString(ActiveKey);
            foreach (var obj in objectsToManage)
                obj.SetActive(obj.name == activeName);
        }

        // Puertas
        foreach (var door in doorsToManage)
            door.LoadState();

        Debug.Log("Estado cargado (objetos normales + puertas).");
    }
    public void ResetAllSavedData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("¡PlayerPrefs reseteados!");
    }

    private void Awake()
    {
        LoadState();
    }
}
