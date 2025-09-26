using UnityEngine;

[System.Serializable]
public class NoteData
{
    [Tooltip("Tiempo en segundos cuando debe aparecer la nota")]
    public float time;

    [Tooltip("Tecla que el jugador debe presionar (A, S o D, Space)")]
    public NoteKey key;

    [Tooltip("Coordenada X en el grid (si aplica)")]
    public int gridX = -1;

    [Tooltip("Coordenada Y en el grid (si aplica)")]
    public int gridY = -1;

    [Tooltip("Color que pintará (opcional)")]
    public Color paintColor = Color.clear;

    [Tooltip("¿Esta nota puede pintar aunque no tenga posición/color?")]
    public bool allowEmptyPaint = false;

    [Tooltip("Valor que debe tener el slider para que el hit sea válido (0 o 1)")]
    [Range(0, 1)]
    public int requiredSliderValue = 0;

}

