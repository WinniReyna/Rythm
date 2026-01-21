using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject mapPanel;

    private IMenuPanel currentPanel;
    private bool isPaused = false;

    private IInputProvider inputProvider;

    public static PauseManager Instance { get; private set; }
    private Stack<IMenuPanel> panelStack = new Stack<IMenuPanel>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inputProvider = new KeyboardInputProvider();
    }

    void Update()
    {
        if (inputProvider.PausePressed())
        {
            IMenuPanel pausePanel = pauseMenuPanel.GetComponent<IMenuPanel>();

            if (currentPanel == pausePanel)
                CloseCurrentPanel();
            else
                OpenPanel(pauseMenuPanel);
        }

        if (inputProvider.MapPressed())
        {
            IMenuPanel mapMenu = mapPanel.GetComponent<IMenuPanel>();

            if (currentPanel == mapMenu)
                CloseCurrentPanel();
            else
                OpenPanel(mapPanel);
        }
    }

    public void ResumeGame()
    {
        IMenuPanel pausePanel = pauseMenuPanel.GetComponent<IMenuPanel>();
        if (currentPanel == pausePanel)
            CloseCurrentPanel();

        Time.timeScale = 1f;
        isPaused = false;
    }


    public IMenuPanel CurrentPanel => currentPanel;

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pauseMenuPanel != null)
            OpenPanel(pauseMenuPanel);
    }

    public void OpenPanel(GameObject panelObject)
    {
        if (panelObject == null) return;

        IMenuPanel panel = panelObject.GetComponent<IMenuPanel>();
        if (panel == null)
        {
            Debug.LogWarning($"El GameObject {panelObject.name} no implementa IMenuPanel");
            return;
        }

        if (currentPanel != null && currentPanel != panel) CloseCurrentPanel();

        currentPanel = panel;
        panelStack.Push(panel);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        currentPanel.Open();
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel == null) return;

        currentPanel.Close();
        panelStack.Pop();

        currentPanel = panelStack.Count > 0 ? panelStack.Peek() : null;

        if (panelStack.Count == 0)
        {
            isPaused = false;
            Time.timeScale = 1f;

            if (PlayerMovement.Instance != null)
                PlayerMovement.Instance.canMove = true;
        }
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

