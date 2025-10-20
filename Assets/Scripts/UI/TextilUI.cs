using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Lean.Localization;

public class TextilUI : MonoBehaviour, IMenuPanel
{
    [Header("Referencias UI")]
    [SerializeField] private Transform imageContainer;
    [SerializeField] private GameObject rawImagePrefab;
    [SerializeField] private TextilDatabase database;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RawImage previewImage;
    [SerializeField] private GameObject infoPanel;

    private void OnEnable() => RefreshUI();

    public void Open()
    {
        gameObject.SetActive(true);
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;
        RefreshUI();

        if (infoPanel != null)
            infoPanel.SetActive(false);

        //Debug.Log("Idioma inicial: " + LeanLocalization.GetFirstCurrentLanguage());
    }

    public void Close()
    {
        gameObject.SetActive(false);
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }

    public void RefreshUI()
    {
        foreach (Transform child in imageContainer)
            Destroy(child.gameObject);

        string folderPath = Path.Combine(Application.persistentDataPath, "SavedGrids");
        if (!Directory.Exists(folderPath))
            return;

        string[] files = Directory.GetFiles(folderPath, "*.png");
        foreach (string file in files)
        {
            byte[] bytes = File.ReadAllBytes(file);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);
            tex.Apply();

            GameObject rawObj = Instantiate(rawImagePrefab, imageContainer);
            RawImage raw = rawObj.GetComponent<RawImage>();
            if (raw != null)
                raw.texture = tex;

            // Suponemos que el nombre del archivo contiene el ID (ej: "textil_01.png")
            int id = ExtractID(Path.GetFileNameWithoutExtension(file));

            // Asignar evento al botón
            Button btn = rawObj.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => OnImageClicked(id));

            var text = rawObj.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
                text.text = Path.GetFileNameWithoutExtension(file);
        }
    }

    private int ExtractID(string filename)
    {
        // Ejemplo: si el nombre es "textil_01" → retorna 1
        string digits = System.Text.RegularExpressions.Regex.Match(filename, @"\d+").Value;
        return int.TryParse(digits, out int id) ? id : -1;
    }

    private void OnImageClicked(int id)
    {
        TextileData data = database.GetTextilByID(id);
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (data != null)
        {
            if (infoPanel != null)
                infoPanel.SetActive(true);

            if (lang == "English")
            {
                titleText.text = data.titleEN;
                descriptionText.text = data.descriptionEN;
            }
            else // Español por defecto
            {
                titleText.text = data.titleES;
                descriptionText.text = data.descriptionES;
            }

            previewImage.texture = data.image;
        }
        else
        {
            Debug.LogWarning($"No se encontró información para el ID {id}");
        }
    }
}
