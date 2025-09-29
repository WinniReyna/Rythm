using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [Tooltip("Clave de localización (Lean Localization)")]
    public string textKey;

    [Tooltip("Icono o retrato del personaje")]
    public Sprite characterIcon;

    [Tooltip("Nombre del personaje (también puede ser una clave de localización)")]
    public string characterName;
}

