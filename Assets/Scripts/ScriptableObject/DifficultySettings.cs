using UnityEngine;

[CreateAssetMenu(fileName = "DifficultySettings", menuName = "Game/Difficulty Settings")]
public class DifficultySettings : ScriptableObject
{
    [Header("Notas")]
    public float noteSpeed = 5f;       // Velocidad de las notas
    public float spawnRateMultiplier = 1f; // Multiplica la cantidad de notas que aparecen
}
