using UnityEngine;

[System.Serializable]
public struct GridPosition
{
    public int x;
    public int y;
}

[System.Serializable]
public class NoteData
{
    [Tooltip("Tiempo en segundos cuando debe aparecer la nota")]
    public float time;

    [Tooltip("Tecla que el jugador debe presionar (A, S o D, Space, shiftLeft)")]
    public NoteKey key;

    [Tooltip("Sprites que pintará al acertar (opcional)")]
    public Sprite[] paintSprites;

    [Tooltip("Posiciones donde se pintará cada sprite")]
    public GridPosition[] paintPositions; 

    [Tooltip("Activar el slider")]
    public bool isSlider = false;

    [HideInInspector]
    public double spawnDspTime;
}
