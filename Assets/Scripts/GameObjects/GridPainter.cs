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

                // Asegurar SpriteRenderer
                var sr = cell.GetComponent<SpriteRenderer>();
                if (sr == null) sr = cell.AddComponent<SpriteRenderer>();

                // Usar el color que elijas en el inspector
                sr.color = defaultColor;

                gridCells[x, y] = sr;
            }
        }
    }

    public void PaintCell(int x, int y, Color color)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridCells[x, y].color = color;
        }
    }
}



