using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float showDelay = 0.8f;

    [SerializeField] private RawImage gridPreview;
    [SerializeField] private GridPainter gridPainter;

    [SerializeField] private AudioSource songAudio;

    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        resultPanel.SetActive(false);
    }

    public void ShowResults()
    {
        StartCoroutine(ShowResultsWithDelay());
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnExitButton()
    {
        FindObjectOfType<GameManager>()?.Exit();
    }

    private IEnumerator ShowResultsWithDelay()
    {
        // Espera el retraso original menos un pequeño margen
        float saveMargin = 0.3f; // segundos antes de mostrar el panel
        yield return new WaitForSeconds(showDelay - saveMargin);

        // Guarda la imagen del grid
        GridPainter gridPainter = FindObjectOfType<GridPainter>();
        StartCoroutine(gridPainter.GenerateGridTexture((Texture2D tex) =>
        {
            if (tex != null)
            {
                gridPreview.texture = tex;
                gridPreview.SetNativeSize();
                gridPreview.gameObject.SetActive(true);
            }
        }));

        // Espera el margen restante para mostrar el panel
        yield return new WaitForSeconds(saveMargin);

        if (scoreManager == null) yield break;

        percentageText.text = $"Aciertos: {scoreManager.GetHitPercentage():0.0}%";
        scoreText.text = $"Puntuación: {scoreManager.GetScore()}";

        resultPanel.SetActive(true);

        if (songAudio != null && songAudio.isPlaying)
            songAudio.Stop();
    }

    public void ReturnToMainLevel()
    {
        if (GameState.Instance != null)
        {
            GameState.Instance.ReturnToMainScene("GameScene");
        }
        else
        {
            // fallback si por algún motivo no hay GameState
            if (LoadingManager.Instance != null)
            {
                LoadingManager.Instance.LoadScene("GameScene");
            }
            else
            {
                // último recurso: carga directa
                LoadingManager.Instance.LoadScene("GameScene");
            }
        }
    }
}


