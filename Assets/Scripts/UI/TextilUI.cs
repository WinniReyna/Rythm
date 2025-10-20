using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class TextilUI : MonoBehaviour, IMenuPanel
{
    [Header("Referencias UI")]
    [SerializeField] private Transform imageContainer;
    [SerializeField] private GameObject rawImagePrefab; // Prefab con un RawImage (y opcionalmente un TMP_Text debajo)

    private void OnEnable()
    {
        RefreshUI();
    }

    public void Open()
    {
        gameObject.SetActive(true);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        RefreshUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }

    public void RefreshUI()
    {
        // Limpia el contenedor
        foreach (Transform child in imageContainer)
            Destroy(child.gameObject);

        // Ruta donde están guardadas las imágenes
        string folderPath = Path.Combine(Application.persistentDataPath, "SavedGrids");
        if (!Directory.Exists(folderPath))
        {
            Debug.LogWarning("No se encontró la carpeta de grids guardados.");
            return;
        }

        // Cargar todos los PNG
        string[] files = Directory.GetFiles(folderPath, "*.png");
        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            tex.Apply();

            // Crear RawImage en el contenedor
            GameObject rawObj = Instantiate(rawImagePrefab, imageContainer);
            RawImage raw = rawObj.GetComponent<RawImage>();
            if (raw != null)
                raw.texture = tex;

            // Mostrar nombre del archivo (opcional)
            var text = rawObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = Path.GetFileNameWithoutExtension(file);
        }

        Debug.Log($"Cargadas {files.Length} imágenes desde: {folderPath}");
    }
}
