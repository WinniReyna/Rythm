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

    void Update()
    {
        if (!gameStarted || resultsShown) return;

        if (noteSpawner != null && noteSpawner.AllNotesFinished())
        {
            resultsShown = true;
            resultPanelUI?.ShowResults();
        }
    }

    public void Retry()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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


