using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int hitNotes = 0;
    private int totalNotes = 0;

    private int pendingPoints = 0;

    private int perfectHits = 0;
    private int goodHits = 0;
    private int badHits = 0;

    private string type;

    public void AddHit(int amount, string hitType)
    {
        hitNotes++;
        pendingPoints += amount;

        switch (hitType)
        {
            case "Perfect!": perfectHits++; break;
            case "Good!": goodHits++; break;
            case "Bad!": badHits++; break;
        }

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

    /// <summary>
    /// Descarta los puntos pendientes de sumar al score. 
    /// Útil si el slider falla.
    /// </summary>
    public void ClearPendingPoints()
    {
        pendingPoints = 0;
        Debug.Log("Puntos pendientes descartados");
    }


    public void CommitPendingPoints()
    {
        score += pendingPoints;
        pendingPoints = 0;
        Debug.Log("Score actualizado: " + score);
    }

    public void SubtractScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0; // evitar puntuación negativa
        Debug.Log("Score restado: " + score);
    }


    public int GetScore() => score;

    //porcentajes por tipo de hit
    public float GetPerfectPercentage()
    {
        if (hitNotes == 0) return 0f;
        return (perfectHits / (float)hitNotes) * 100f;
    }

    public float GetGoodPercentage()
    {
        if (hitNotes == 0) return 0f;
        return (goodHits / (float)hitNotes) * 100f;
    }

    public float GetBadPercentage()
    {
        if (hitNotes == 0) return 0f;
        return (badHits / (float)hitNotes) * 100f;
    }

    public void ResetHitCounts()
    {
        perfectHits = 0;
        goodHits = 0;
        badHits = 0;
    }

    public void CalculateHitPercentages(int totalNotes)
    {
        int missedNotes = totalNotes - (perfectHits + goodHits + badHits);
        badHits += Mathf.Max(0, missedNotes);

        float perfectPercent = (perfectHits / (float)totalNotes) * 100f;
        float goodPercent = (goodHits / (float)totalNotes) * 100f;
        float badPercent = (badHits / (float)totalNotes) * 100f;

        Debug.Log($"Antes del slider - Perfect: {perfectPercent:F1}% | Good: {goodPercent:F1}% | Bad: {badPercent:F1}%");

        if (perfectPercent >= goodPercent && perfectPercent >= badPercent) type = "Perfect!";
        if (goodPercent >= perfectPercent && goodPercent >= badPercent) type = "Good!";
        if (badPercent >= perfectPercent && badPercent >= goodPercent) type = "Bad!";
    }

    public string GetMostFrequentHitBeforeSlider()
    {
        return type;
    }
}


