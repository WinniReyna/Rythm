using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public string npcName;
    public DialogueLine[] lines;

}

[System.Serializable]
public class DialogueLine
{
    [Tooltip("ID de Lean Localization")]
    public string localizedKey;        // Para Lean Localization
    public Texture2D icon;             // Icono opcional
    public AudioClip npcVoice;         // Audio opcional
    public DialogueResponse[] responses;
}

[System.Serializable]
public class DialogueResponse
{
    public string localizedKey;        // Texto de la respuesta
    public DialogueData nextDialogue;  // Siguiente diálogo si se selecciona
}






