using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject mapPanel;

    [Header("Panels que no deben cerrar la pausa")]
    [SerializeField] private GameObject[] internalUIPanels;

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

    private void Update()
    {
        // ESC pressed
        if (inputProvider.PausePressed())
        {
            IMenuPanel pausePanel = pauseMenuPanel.GetComponent<IMenuPanel>();

            // Si el current panel es un panel interno cerrar TODO
            if (currentPanel != null && IsInternalUIPanel(currentPanel))
            {
                CloseAllPanels();
            }
            else
            {
                // Abrir/cerrar pausa normal
                if (currentPanel == pausePanel)
                    CloseCurrentPanel();
                else
                    OpenPanel(pauseMenuPanel);
            }
        }

        // Map 
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

    private bool IsInternalUIPanel(IMenuPanel panel)
    {
        foreach (var go in internalUIPanels)
        {
            if (go == null) continue;
            if (go.GetComponent<IMenuPanel>() == panel)
                return true;
        }
        return false;
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
        if (panel == null) return;

        if (currentPanel != null && currentPanel != panel)
        {
            if (!(currentPanel == pauseMenuPanel.GetComponent<IMenuPanel>() && IsInternalUIPanel(panel)))
                CloseCurrentPanel();
        }

        currentPanel = panel;
        panelStack.Push(panel);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        currentPanel.Open();

        Time.timeScale = panelStack.Contains(pauseMenuPanel.GetComponent<IMenuPanel>()) ? 0f : 1f;
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel == null) return;

        currentPanel.Close();
        panelStack.Pop();

        currentPanel = panelStack.Count > 0 ? panelStack.Peek() : null;

        if (panelStack.Count == 0)
        {
            if (PlayerMovement.Instance != null)
                PlayerMovement.Instance.canMove = true;

            Time.timeScale = 1f;
        }
        else
        {
            if (PlayerMovement.Instance != null)
                PlayerMovement.Instance.canMove = false;

            Time.timeScale = panelStack.Contains(pauseMenuPanel.GetComponent<IMenuPanel>()) ? 0f : 1f;
        }
    }

    // cerrar todos los paneles abiertos
    public void CloseAllPanels()
    {
        while (panelStack.Count > 0)
        {
            IMenuPanel panel = panelStack.Pop();
            panel.Close();
        }

        currentPanel = null;

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;

        Time.timeScale = 1f;
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

