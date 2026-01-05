using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private NoteSpawner noteSpawner;
    [SerializeField] private ResultPanelUI resultPanelUI;
    [SerializeField] private GameObject gameOverPanel;

    [Header("Textil actual")]
    public TextileData currentTextile;

    private bool resultsShown = false;
    private bool gameStarted = false;
    private int failedSliders = 0;
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentTextile(TextileData textile)
    {
        currentTextile = textile;
    }

    public void OnGameStarted()
    {
        gameStarted = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        failedSliders = 0;
        gameOver = false;
        resultsShown = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowResultsPanel()
    {
        if (resultsShown) return;

        resultsShown = true;
        Debug.Log("Mostrando Result Panel");
        resultPanelUI?.ShowResults();
    }

    public void RegisterFailedSlider()
    {
        if (gameOver) return;

        failedSliders++;
        Debug.Log($"Slider fallado");

        if (failedSliders >= 3)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        gameOver = true;
        Debug.Log("GAME OVER");

        noteSpawner?.StopAllCoroutines();

        var beatSpawner = FindObjectOfType<BeatNoteSpawner>();
        if (beatSpawner != null)
            beatSpawner.StopMusic();

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void LoadCinematic()
    {
        Time.timeScale = 1f;

        if (currentTextile != null && !string.IsNullOrEmpty(currentTextile.cinematicScene))
        {
            SceneManager.LoadScene(currentTextile.cinematicScene);
        }
        else
        {
            Debug.LogWarning("No hay escena de cinemática asignada al TextileData");
        }
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}



