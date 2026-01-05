using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCinematicIfPlayed : MonoBehaviour
{
    string cinematicID => SceneManager.GetActiveScene().name;

    void Awake()
    {

        //PlayerPrefs.DeleteKey(cinematicID);

        if (PlayerPrefs.GetInt(cinematicID, 0) == 1)
        {
            // Ya se vio regresar directo al juego
            GameState.Instance.ReturnToMainScene("GameScene");
        }
    }
}

