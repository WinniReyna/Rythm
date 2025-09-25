using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int hitNotes = 0;
    private int totalNotes = 0;

    public void AddHit(int amount)
    {
        hitNotes++;
        score += amount;
        Debug.Log("Puntuación: " + score);
    }

    public void AddMiss(int amount)
    {
        score += amount;
        Debug.Log("Puntuación: " + score);
    }

    public float GetHitPercentage()
    {
        if (totalNotes == 0) return 0f;
        return (hitNotes / (float)totalNotes) * 100f;
    }

    public void SetTotalNotes(int total)
    {
        totalNotes = total;
    }

    public int GetScore() => score;
}


