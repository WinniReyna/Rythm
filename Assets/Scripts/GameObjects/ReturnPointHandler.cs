using UnityEngine;
using System.Collections;

public class ReturnPointHandler : MonoBehaviour
{
    [Header("Intervalo de guardado automático (segundos)")]
    [SerializeField] private float autoSaveInterval = 30f;

    private JsonSaveManager saveManager;

    private void Awake()
    {
        saveManager = FindObjectOfType<JsonSaveManager>();
        if (saveManager == null)
        {
            Debug.LogError("No se encontró JsonSaveManager en la escena.");
            return;
        }

        // Restaurar estado al iniciar
        saveManager.LoadGameState();

        // Iniciar corrutina de guardado automático
        StartCoroutine(AutoSaveCoroutine());
    }

    private IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);
            SaveGameState();
        }
    }

    public void SaveGameState()
    {
        if (saveManager != null)
        {
            saveManager.SaveGameState();
            Debug.Log("Juego guardado mediante ReturnPointHandler.");
        }
    }

    public void LoadGameState()
    {
        if (saveManager != null)
        {
            saveManager.LoadGameState();
            Debug.Log("Juego cargado mediante ReturnPointHandler.");
        }
    }

    public void ResetSave()
    {
        if (saveManager != null)
        {
            saveManager.ResetSave();
            Debug.Log("Archivo de guardado eliminado mediante ReturnPointHandler.");
        }
    }
}

