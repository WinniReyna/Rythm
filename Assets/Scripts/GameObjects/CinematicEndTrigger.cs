using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicEndTrigger : MonoBehaviour
{
    [Tooltip("NPCSceneData que inició esta cinemática")]
    [SerializeField] private NPCSceneData sceneData;

    // Este método se llama al terminar la cinemática
    public void OnCinematicFinished()
    {
        if (sceneData == null)
        {
            Debug.LogError("No hay NPCSceneData asignado.");
            return;
        }

        // Si hay minijuego definido, lo cargamos
        if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
        {
            LoadingManager.Instance.LoadScene(sceneData.minigameSceneName);
            return;
        }

        // Si no hay minijuego, volvemos a la escena principal
        GameState.Instance.ReturnToMainScene("MainScene"); // Cambia por tu nombre real de la escena
    }
}

