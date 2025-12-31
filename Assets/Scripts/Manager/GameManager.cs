using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NoteSpawner noteSpawner;
    [SerializeField] private ResultPanelUI resultPanelUI;
    [SerializeField] private GameObject gameOverPanel;

    private bool resultsShown = false;
    private bool gameStarted = false;
    private int failedSliders = 0;
    private bool gameOver = false;

    public void OnGameStarted()
    {
        gameStarted = true;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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

        // Detener gameplay
        noteSpawner?.StopAllCoroutines();

        // Detener música
        var beatSpawner = FindObjectOfType<BeatNoteSpawner>();
        if (beatSpawner != null)
            beatSpawner.StopMusic();

        // Mostrar UI
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }


    public bool IsGameOver()
    {
        return gameOver;
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


