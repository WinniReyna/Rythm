using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;

    private IMenuPanel currentPanel;
    private bool isPaused = false;

    private IInputProvider inputProvider;

    private void Awake()
    {
        inputProvider = new KeyboardInputProvider();
    }

    void Update()
    {
        if (inputProvider.PausePressed())
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);

        if (currentPanel != null)
            currentPanel.Close();
    }

    public void OpenPanel(GameObject panelObject)
    {
        IMenuPanel panel = panelObject.GetComponent<IMenuPanel>();

        if (currentPanel != null)
            currentPanel.Close();

        currentPanel = panel;
        currentPanel.Open();
    }

    public void CloseCurrentPanel()
    {
        currentPanel.Close();          
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        LoadingManager.Instance.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}

