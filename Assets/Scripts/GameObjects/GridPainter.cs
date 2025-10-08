using UnityEngine;

public class GridPainter : MonoBehaviour
{
    [Header("Tamaño del grid")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private float cellSize = 1f;

    [Header("Prefab de celda (debe tener SpriteRenderer y GridCell)")]
    [SerializeField] private GameObject cellPrefab;

    [Header("Sprites a colocar manualmente")]
    [Tooltip("Define qué sprite irá en qué celda (x, y).")]
    public GridSpriteData[] gridSprites;

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

                // Asigna sprite si hay uno definido para esta posición
                Sprite sprite = GetSpriteForPosition(x, y);
                if (sprite != null)
                    gridCell.SetSprite(sprite);
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

    private Sprite GetSpriteForPosition(int x, int y)
    {
        foreach (var data in gridSprites)
        {
            if (data.x == x && data.y == y)
                return data.sprite;
        }
        return null;
    }

    private bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }
}
