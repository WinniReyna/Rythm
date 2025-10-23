using UnityEngine;
using Lean.Localization;

[CreateAssetMenu(fileName = "ExamineData", menuName = "Examine/Examine Data")]
public class ExamineData : ScriptableObject
{
    public Texture2D objectTexture;    // Textura a mostrar en RawImage

    [Header("Información en Español")]
    public string titleES;
    [TextArea] public string descriptionES;

    [Header("Información en Inglés")]
    public string titleEN;
    [TextArea] public string descriptionEN;

    // Método para obtener el título según el idioma actual
    public string GetTitle()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return titleEN;
        return titleES;
    }

    // Método para obtener la descripción según el idioma actual
    public string GetDescription()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return descriptionEN;
        return descriptionES;
    }
}
