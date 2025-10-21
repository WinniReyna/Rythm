using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    [HideInInspector] public NPCSceneData currentNPCSceneData;
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

    // Llamado para iniciar cinemática o minijuego
    public void TriggerScene(NPCSceneData sceneData)
    {
        currentNPCSceneData = sceneData;
        returningFromEvent = false;

        if (!string.IsNullOrEmpty(sceneData.cinematicSceneName))
            SceneManager.LoadScene(sceneData.cinematicSceneName);
        else if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
            SceneManager.LoadScene(sceneData.minigameSceneName);
    }

    // Llamado desde cinemática o minijuego al terminar
    public void ReturnToMainScene(string mainSceneName)
    {
        returningFromEvent = true;
        SceneManager.LoadScene(mainSceneName);
    }
}
