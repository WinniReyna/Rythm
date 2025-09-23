using UnityEngine;

public class Note : MonoBehaviour, INote
{
    public float speed = 5f;

    private NoteKey requiredKey;
    private ScoreManager scoreManager;
    private GridPainter gridPainter;
    private int gridX;
    private int gridY;
    private Color paintColor = Color.white;

    private bool allowEmptyPaint = false;

    // Campo para evitar usar Find en Update repetidamente (opcional)
    private bool initialized = false;

    // IMPLEMENTACIÓN REQUERIDA POR INOTE
    public void Initialize(NoteKey key)
    {
        // Inicialización por defecto si se llama vía interfaz
        Initialize(key, 0, 0, Color.white);
    }

    // Sobrecarga que usa el spawner para pasar coordenadas y color
    public void Initialize(NoteKey key, int x = -1, int y = -1, Color? color = null, bool allowEmpty = false)
    {
        requiredKey = key;
        gridX = x;
        gridY = y;
        paintColor = color ?? Color.clear;
        allowEmptyPaint = allowEmpty;

        scoreManager = FindObjectOfType<ScoreManager>();
        gridPainter = FindObjectOfType<GridPainter>();
    }

    void Update()
    {
        // Mueve la nota aunque no esté inicializada (puedes cambiar esto si lo prefieres)
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Miss();
        }
    }

    public void Hit()
    {
        Debug.Log($"Nota acertada: {requiredKey}");

        // Si la bandera es false, no pintamos NUNCA
        if (allowEmptyPaint)
        {
            bool hasValidPosition = gridX >= 0 && gridY >= 0;
            bool hasValidColor = paintColor.a > 0f;

            if (hasValidPosition && hasValidColor)
            {
                gridPainter?.PaintCell(gridX, gridY, paintColor);
            }
            else
            {
                // Si allowEmptyPaint = true pero no tiene color o posición, pintamos con default
                gridPainter?.PaintCell(gridX >= 0 ? gridX : 0, gridY >= 0 ? gridY : 0, paintColor != Color.clear ? paintColor : gridPainter.defaultColor);
            }
        }
        else
        {
            // NO pintar
            Debug.Log("Esta nota NO pinta el grid.");
        }

        // Sumar puntos
        scoreManager?.AddScore(100);

        Destroy(gameObject);
    }



    public void Miss()
    {
        Debug.Log("Nota fallida: " + requiredKey);
        if (scoreManager != null)
            scoreManager.AddScore(-50);
        Destroy(gameObject);
    }
}






