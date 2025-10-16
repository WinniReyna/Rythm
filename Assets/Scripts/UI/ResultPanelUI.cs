using UnityEngine;
using TMPro;
using System.Collections;

public class ResultPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float showDelay = 0.8f;

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
        FindObjectOfType<GameManager>()?.Retry();
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
        if (gridPainter != null)
        {
            gridPainter.SaveGrid(); // Esto ejecuta la captura
            Debug.Log("Grid guardado antes de mostrar resultados.");
        }

        // Espera el margen restante para mostrar el panel
        yield return new WaitForSeconds(saveMargin);

        if (scoreManager == null) yield break;

        percentageText.text = $"Aciertos: {scoreManager.GetHitPercentage():0.0}%";
        scoreText.text = $"Puntuación: {scoreManager.GetScore()}";

        resultPanel.SetActive(true);
    }
}


