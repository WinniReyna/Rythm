using UnityEngine;

[System.Serializable]
public class NoteData
{
    [Tooltip("Tiempo en segundos cuando debe aparecer la nota")]
    public float time;

    [Tooltip("Tecla que el jugador debe presionar (A, S o D, Space, shiftLeft)")]
    public NoteKey key;

    [Tooltip("Coordenada X en el grid (si aplica)")]
    public int gridX = -1;

    [Tooltip("Coordenada Y en el grid (si aplica)")]
    public int gridY = -1;

    [Tooltip("¿Esta nota puede pintar aunque no tenga posición/color?")]
    public bool allowPaint = false;

    [Tooltip("Sprite que pintará al acertar (opcional)")]
    public Sprite paintSprite;

    [Tooltip("Activar el slider")]
    public bool isSlider = false;
}

