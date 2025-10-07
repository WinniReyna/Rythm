using UnityEngine;

public class GridPainter : MonoBehaviour
{
    [Header("Tamaño del grid")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private float cellSize = 1f;

    [Header("Prefab de celda")]
    [SerializeField] private GameObject cellPrefab; // Debe tener SpriteRenderer

    [Header("Color inicial de las celdas")]
    public Color defaultColor = Color.black; // editable en el inspector

    private SpriteRenderer[,] gridCells;

    void Start()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("GridPainter: No se ha asignado el prefab de celda!");
            return;
        }

        gridCells = new SpriteRenderer[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Posición local, relativo al GridPainter
                Vector3 localPos = new Vector3(x * cellSize, y * cellSize, 0);

                var cell = Instantiate(cellPrefab, transform);
                cell.transform.localPosition = localPos;
                cell.name = $"Cell_{x}_{y}";

                // Asegurar que tenga SpriteRenderer
                var sr = cell.GetComponent<SpriteRenderer>();
                if (sr == null) sr = cell.AddComponent<SpriteRenderer>();

                sr.color = defaultColor;

                gridCells[x, y] = sr;
            }
        }
    }

    /// <summary>
    /// Pinta una celda con un color (opcional)
    /// </summary>
    public void PaintCell(int x, int y, Color color)
    {
        if (IsValidCell(x, y))
            gridCells[x, y].color = color;
    }

    /// <summary>
    /// Pinta una celda con un sprite
    /// </summary>
    public void PaintCellWithSprite(int x, int y, Sprite sprite)
    {
        if (IsValidCell(x, y) && sprite != null)
            gridCells[x, y].sprite = sprite;
    }

    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}

