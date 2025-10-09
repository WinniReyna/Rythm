using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NoteSpawner noteSpawner;
    [SerializeField] private ResultPanelUI resultPanelUI;

    private bool resultsShown = false;
    private bool gameStarted = false;

    public void OnGameStarted()
    {
        gameStarted = true;
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void ShowResultsPanel()
    {
        if (resultsShown) return;

        resultsShown = true;
        Debug.Log("Mostrando Result Panel");
        resultPanelUI?.ShowResults();
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


