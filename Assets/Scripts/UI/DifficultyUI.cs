using UnityEngine;

public class DifficultyUI : MonoBehaviour, IMenuPanel
{
    [SerializeField] private GameObject difficultyPanel;

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ChooseDifficulty(DifficultySettings difficulty)
    {
        DifficultyManager.Instance.SetDifficulty(difficulty);
        Debug.Log("Dificultad elegida: " + difficulty.name);
    }
}
