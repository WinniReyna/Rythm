using UnityEngine;

public class GridPainter : MonoBehaviour
{
    [Header("Tamaño del grid")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private float cellSize = 1f;

    [Header("Prefab de celda")]
    [SerializeField] private GameObject cellPrefab; // Debe tener SpriteRenderer y GridCell

    private GridCell[,] gridCells;

    void Start()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("GridPainter: No se ha asignado el prefab de celda!");
            return;
        }

        gridCells = new GridCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 localPos = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cellObj = Instantiate(cellPrefab, transform);
                cellObj.transform.localPosition = localPos;
                cellObj.name = $"Cell_{x}_{y}";

                GridCell gridCell = cellObj.GetComponent<GridCell>();
                if (gridCell == null) gridCell = cellObj.AddComponent<GridCell>();

                gridCells[x, y] = gridCell;
            }
        }
    }

    /// <summary>
    /// Pinta la celda en la posición (x, y) con un sprite.
    /// </summary>
    public void PaintCellWithSprite(int x, int y, Sprite sprite)
    {
        if (IsValidCell(x, y) && sprite != null)
        {
            gridCells[x, y].SetSprite(sprite);
        }
    }

    /// <summary>
    /// Resetea la celda a su sprite original
    /// </summary>
    public void ResetCell(int x, int y)
    {
        if (IsValidCell(x, y))
        {
            gridCells[x, y].ResetCell();
        }
    }

    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
