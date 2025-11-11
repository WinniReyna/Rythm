using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [Header("Identificación del NPC")]
    public string npcNameES;
    public string npcNameEN;

    [Header("Líneas de diálogo")]
    public DialogueLine[] lines;

    public string GetNpcName()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return npcNameEN;
        return npcNameES;
    }
}






