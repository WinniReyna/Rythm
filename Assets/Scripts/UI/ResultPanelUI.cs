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
        yield return new WaitForSeconds(showDelay);

        if (scoreManager == null) yield break;

        percentageText.text = $"Aciertos: {scoreManager.GetHitPercentage():0.0}%";
        scoreText.text = $"Puntuación: {scoreManager.GetScore()}";

        resultPanel.SetActive(true);
    }
}


