using UnityEngine;
using System.Collections.Generic;

public class SceneStateManager : MonoBehaviour
{
    [Header("Objetos a controlar")]
    [SerializeField] private List<GameObject> objectsToManage;

    private const string ActiveKey = "ActiveObject";

    // Guardar el objeto activo
    public void SaveState()
    {
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
        {
            PlayerPrefs.SetString(ActiveKey, activeObj.name);
        }

        PlayerPrefs.Save();
        Debug.Log("Estado guardado. Objeto activo: " + (activeObj != null ? activeObj.name : "ninguno"));
    }

    // Cargar estado
    public void LoadState()
    {
        if (!PlayerPrefs.HasKey(ActiveKey)) return;

        string activeName = PlayerPrefs.GetString(ActiveKey);

        foreach (var obj in objectsToManage)
        {
            obj.SetActive(obj.name == activeName);
        }

        Debug.Log("Estado cargado. Objeto activo: " + activeName);
    }

    // cargar automáticamente al iniciar
    private void Awake()
    {
        LoadState();
    }
}
