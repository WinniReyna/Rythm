using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    [Header("Texto de respuesta")]
    [TextArea] public string textES;
    [TextArea] public string textEN;

    [Header("Siguiente diálogo al seleccionar")]
    public DialogueData nextDialogue;

    public string GetText()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return textEN;
        return textES;
    }
}
