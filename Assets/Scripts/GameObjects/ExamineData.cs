using UnityEngine;
using Lean.Localization;

[CreateAssetMenu(fileName = "ExamineData", menuName = "Examine/Examine Data")]
public class ExamineData : ScriptableObject
{
    public Texture2D objectTexture;    

    [Header("Información en Español")]
    public string titleES;
    [TextArea] public string descriptionES;

    [Header("Información en Inglés")]
    public string titleEN;
    [TextArea] public string descriptionEN;

    public string GetTitle()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return titleEN;
        return titleES;
    }

    public string GetDescription()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return descriptionEN;
        return descriptionES;
    }
}
