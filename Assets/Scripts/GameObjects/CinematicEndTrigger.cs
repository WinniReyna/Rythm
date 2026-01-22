using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicEndTrigger : MonoBehaviour
{
    private string cinematicID => SceneManager.GetActiveScene().name;

    public void OnCinematicFinished()
    {

        PlayerPrefs.SetInt(cinematicID, 1);
        PlayerPrefs.Save();

        GameState.Instance.ReturnToMainScene("GameScene");
    }
}


