using UnityEngine;

public class DifficultyUI : MonoBehaviour
{
    public NoteSpawner noteSpawner;

    public GameObject difficultyPanel; // Panel que contiene los botones

    public void ChooseDifficulty(DifficultySettings difficulty)
    {
        noteSpawner.StartGame(difficulty);

        // Apagar la UI
        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);

        Debug.Log("Dificultad elegida: " + difficulty.name);
    }
}


