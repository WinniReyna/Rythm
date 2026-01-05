using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicEndTrigger : MonoBehaviour
{
    // Usamos el nombre de la escena actual como ID único
    private string cinematicID => SceneManager.GetActiveScene().name;

    // Se llama al terminar la cinemática
    public void OnCinematicFinished()
    {
        // Marcar la cinemática como vista
        PlayerPrefs.SetInt(cinematicID, 1);
        PlayerPrefs.Save();

        // Regresar al nivel principal
        GameState.Instance.ReturnToMainScene("GameScene");
    }
}


