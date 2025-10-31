using UnityEngine;

[CreateAssetMenu(fileName = "TextilData", menuName = "Database/Textil Data")]
public class TextileData : ScriptableObject
{
    public int id;

    [Header("Escena asociada (opcional)")]
    public string sceneName;

    [Header("Información en Español")]
    public string titleES;
    [TextArea] public string descriptionES;

    [Header("Información en Inglés")]
    public string titleEN;
    [TextArea] public string descriptionEN;

    public Texture2D image;

    

    // Método para obtener el texto según el idioma actual
    public string GetTitle()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return titleEN;
        return titleES;
    }

    public string GetDescription()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return descriptionEN;
        return descriptionES;
    }
}

