using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntuaci�n: " + score);
    }
}
