using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private IMenuPanel currentPanel;
    [SerializeField] private string cinematicID = "Cinematic0";

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
    }

    public void OpenPanel(GameObject panelObject)
    {
        IMenuPanel panel = panelObject.GetComponent<IMenuPanel>();

        if (currentPanel != null)
            currentPanel.Close();

        currentPanel = panel;
        currentPanel.Open();
    }

    public void PlayGame()
    {
        if (PlayerPrefs.GetInt(cinematicID, 0) == 1)
        {
            // Ya se vio vamos directo al juego
            LoadingManager.Instance.LoadScene("GameScene");
        }
        else
        {
            // No se ha visto cargamos la cinemática
            LoadingManager.Instance.LoadScene(cinematicID);
        }
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void CloseCurrentPanel()
    {
        currentPanel.Close();
    }
}

