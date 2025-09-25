using UnityEngine;
using TMPro;

public class ResultPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        resultPanel.SetActive(false);
    }

    public void ShowResults()
    {
        if (scoreManager == null) return;

        percentageText.text = $"Aciertos: {scoreManager.GetHitPercentage():0.0}%";
        scoreText.text = $"Puntuación: {scoreManager.GetScore()}";

        resultPanel.SetActive(true);
    }

    public void OnRetryButton()
    {
        FindObjectOfType<GameManager>()?.Retry();
    }

    public void OnExitButton()
    {
        FindObjectOfType<GameManager>()?.Exit();
    }
}


