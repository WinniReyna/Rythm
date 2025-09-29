using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private IMenuPanel currentPanel;

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
        SceneManager.LoadScene("GameScene");
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

