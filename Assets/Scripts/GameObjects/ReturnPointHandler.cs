using UnityEngine;
using System.Collections;

public class ReturnPointHandler : MonoBehaviour
{
    [Header("Intervalo de guardado automático (segundos)")]
    [SerializeField] private float autoSaveInterval = 30f;
    private const string PlayerPosX = "PlayerPosX";
    private const string PlayerPosY = "PlayerPosY";
    private const string PlayerPosZ = "PlayerPosZ";

    private void Awake()
    {
        // Restaurar estado al iniciar
        LoadGameState();

        // Iniciar corrutina de guardado automático
        StartCoroutine(AutoSaveCoroutine());
    }

    /// <summary>
    /// Corrutina que guarda el estado cada intervalo de tiempo
    /// </summary>
    private IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveInterval);
            SaveGameState();
        }
    }

    /// <summary>
    /// Guardar estado del jugador y objetos
    /// </summary>
    public void SaveGameState()
    {
        StartCoroutine(SaveGameStateCoroutine());
    }

    private IEnumerator SaveGameStateCoroutine()
    {
        // Espera un frame para asegurar que todo esté actualizado
        yield return null;

        var player = FindPlayer();
        if (player != null)
        {
            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat(PlayerPosX, pos.x);
            PlayerPrefs.SetFloat(PlayerPosY, pos.y);
            PlayerPrefs.SetFloat(PlayerPosZ, pos.z);
            Debug.Log("Posición del jugador guardada: " + pos);
        }

        var sceneStateManager = FindObjectOfType<SceneStateManager>();
        if (sceneStateManager != null)
        {
            sceneStateManager.SaveState();
            Debug.Log("Estado de los objetos guardado desde ReturnPointHandler.");
        }

        PlayerPrefs.Save();
    }

    /// <summary>
    /// Restaurar estado del jugador y objetos
    /// </summary>
    public void LoadGameState()
    {
        var player = FindPlayer();
        if (player != null)
        {
            float x = PlayerPrefs.GetFloat(PlayerPosX, player.transform.position.x);
            float y = PlayerPrefs.GetFloat(PlayerPosY, player.transform.position.y);
            float z = PlayerPrefs.GetFloat(PlayerPosZ, player.transform.position.z);
            player.transform.position = new Vector3(x, y, z);
            player.canMove = true;
            Debug.Log("Posición del jugador restaurada: " + player.transform.position);
        }

        var sceneStateManager = FindObjectOfType<SceneStateManager>();
        if (sceneStateManager != null)
        {
            sceneStateManager.LoadState();
            Debug.Log("Estado de los objetos restaurado desde ReturnPointHandler.");
        }
    }

    /// <summary>
    /// Método confiable para obtener el jugador
    /// </summary>
    private PlayerMovement FindPlayer()
    {
        if (PlayerMovement.Instance != null)
            return PlayerMovement.Instance;

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            return playerObj.GetComponent<PlayerMovement>();

        return FindObjectOfType<PlayerMovement>();
    }
}

