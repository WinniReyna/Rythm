using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Header("Texto de diálogo")]
    [TextArea] public string textES;
    [TextArea] public string textEN;

    [Header("Icono y voz opcional")]
    public Texture2D icon;
    public AudioClip npcVoice;

    [Header("Respuestas del jugador (opcional)")]
    public DialogueResponse[] responses;

    public string GetText()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return textEN;
        return textES;
    }
}
