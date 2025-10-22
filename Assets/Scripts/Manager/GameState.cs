using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [HideInInspector] public NPCSceneData currentNPCSceneData;
    [HideInInspector] public Vector3 playerPosition;
    [HideInInspector] public string lastNPCName;
    [HideInInspector] public bool returningFromEvent = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void TriggerScene(NPCSceneData sceneData, Transform player, string npcName)
    {
        currentNPCSceneData = sceneData;
        returningFromEvent = false;

        if (player != null)
            playerPosition = player.position;

        lastNPCName = npcName;

        if (!string.IsNullOrEmpty(sceneData.cinematicSceneName))
            LoadingManager.Instance.LoadScene(sceneData.cinematicSceneName);
        else if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
            LoadingManager.Instance.LoadScene(sceneData.minigameSceneName);
    }

    public void ReturnToMainScene(string mainSceneName)
    {
        returningFromEvent = true;

        if (!string.IsNullOrEmpty(mainSceneName) && LoadingManager.Instance != null)
        {
            // Usamos el LoadingManager para mostrar la UI y cargar la escena
            LoadingManager.Instance.LoadScene(mainSceneName);
        }
        else
        {
            // fallback directo si no hay LoadingManager
            UnityEngine.SceneManagement.SceneManager.LoadScene(mainSceneName);
        }
    }

}
