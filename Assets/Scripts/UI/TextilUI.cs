using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Lean.Localization;
using UnityEngine.SceneManagement;

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

    private Button openSceneButton;
    private TextileData selectedTextile;

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

    /// <summary>
    /// Carga la escena asociada al libro (si tiene alguna)
    /// </summary>
    private void LoadBookScene(TextileData textile)
    {
        if (textile == null || string.IsNullOrEmpty(textile.sceneName))
        {
            Debug.LogWarning("El textil no tiene una escena asociada.");
            return;
        }

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        LoadingManager.Instance.LoadScene(textile.sceneName);
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
        selectedTextile = data; // Guardamos el textil seleccionado

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
            else
            {
                titleText.text = data.titleES;
                descriptionText.text = data.descriptionES;
            }

            previewImage.texture = data.image;

            openSceneButton = GameObject.Find("OpenSceneButton")?.GetComponent<Button>();
            if (openSceneButton == null)
                Debug.LogWarning("No se encontró un botón llamado 'OpenSceneButton' en la escena.");

            // Configuramos el botón global **al seleccionar el textil**
            if (openSceneButton != null)
            {
                openSceneButton.onClick.RemoveAllListeners();
                openSceneButton.onClick.AddListener(() => LoadBookScene(selectedTextile));
                openSceneButton.gameObject.SetActive(!string.IsNullOrEmpty(selectedTextile.sceneName));
            }
        }
        else
        {
            Debug.LogWarning($"No se encontró información para el ID {id}");
            if (openSceneButton != null)
                openSceneButton.gameObject.SetActive(false);
        }
    }
}
