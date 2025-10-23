using UnityEngine;
using Lean.Localization;

[CreateAssetMenu(fileName = "ExamineData", menuName = "Examine/Examine Data")]
public class ExamineData : ScriptableObject
{
    public Texture2D objectTexture;    // Textura a mostrar en RawImage

    [Header("Informaci�n en Espa�ol")]
    public string titleES;
    [TextArea] public string descriptionES;

    [Header("Informaci�n en Ingl�s")]
    public string titleEN;
    [TextArea] public string descriptionEN;

    // M�todo para obtener el t�tulo seg�n el idioma actual
    public string GetTitle()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return titleEN;
        return titleES;
    }

    // M�todo para obtener la descripci�n seg�n el idioma actual
    public string GetDescription()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return descriptionEN;
        return descriptionES;
    }
}
