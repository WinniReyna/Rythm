using UnityEngine;
using System.IO;
using System.Collections;

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
    /// Resetea la celda a su sprite original.
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

    /// <summary>
    /// Salvar el grid en una imagen
    /// </summary>
    [ContextMenu("Guardar imagen del Grid")]
    public IEnumerator GenerateGridTexture(System.Action<Texture2D> callback)
    {
        yield return new WaitForEndOfFrame();

        // Crear cámara y setup
        GameObject camObj = new GameObject("GridCaptureCamera");
        Camera gridCam = camObj.AddComponent<Camera>();
        gridCam.backgroundColor = Color.clear;
        gridCam.clearFlags = CameraClearFlags.SolidColor;
        gridCam.orthographic = true;
        gridCam.cullingMask = LayerMask.GetMask("GridCapture");

        int texWidth = Mathf.RoundToInt(width * 32);
        int texHeight = Mathf.RoundToInt(height * 32);
        RenderTexture rt = new RenderTexture(texWidth, texHeight, 24);
        gridCam.targetTexture = rt;

        // Calcular centro y tamaño
        Vector3 gridCenter = transform.position + new Vector3((width * cellSize) / 2f - cellSize / 2f, (height * cellSize) / 2f - cellSize / 2f, 0);
        camObj.transform.position = gridCenter + new Vector3(0, 0, -10f);

        float aspect = (float)texWidth / texHeight;
        float gridHeight = height * cellSize;
        float gridWidth = width * cellSize;
        float sizeY = gridHeight / 2f;
        float sizeX = gridWidth / (2f * aspect);
        gridCam.orthographicSize = Mathf.Max(sizeY, sizeX);

        SetLayerRecursively(gameObject, LayerMask.NameToLayer("GridCapture"));

        // Render y esperar
        gridCam.Render();
        yield return null; // Espera un frame extra
        RenderTexture.active = rt;

        // Ahora sí, leer píxeles
        Texture2D tex = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, texWidth, texHeight), 0, 0);
        tex.Apply();

        // Limpieza
        RenderTexture.active = null;
        gridCam.targetTexture = null;
        Destroy(rt);
        Destroy(camObj);
        SetLayerRecursively(gameObject, 0); // Devuelve al Default layer

        // Guardar
        string folderPath = Path.Combine(Application.dataPath, "SavedGrids");
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        string fileName = $"{gameObject.name}_Grid.png";
        string fullPath = Path.Combine(folderPath, fileName);
        File.WriteAllBytes(fullPath, tex.EncodeToPNG());

        Debug.Log($"Grid guardado correctamente: {fullPath}");

        callback?.Invoke(tex);
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0.7f, 1f, 0.3f);

        // Centro del grid
        Vector3 gridCenter = transform.position +
            new Vector3((width * cellSize) / 2f - cellSize / 2f, (height * cellSize) / 2f - cellSize / 2f, 0);

        Vector3 size = new Vector3(width * cellSize, height * cellSize, 0.1f);
        Gizmos.DrawCube(gridCenter, size);

        Gizmos.color = new Color(0f, 0.7f, 1f, 1f);
        Gizmos.DrawWireCube(gridCenter, size);

        UnityEditor.Handles.Label(gridCenter + Vector3.up * (height * cellSize / 2f + 0.2f), "Área capturada por cámara");
    }
#endif

}
