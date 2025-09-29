using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    [Tooltip("Clave de localizaci�n (Lean Localization)")]
    public string textKey;

    [Tooltip("Icono o retrato del personaje")]
    public Sprite characterIcon;

    [Tooltip("Nombre del personaje (tambi�n puede ser una clave de localizaci�n)")]
    public string characterName;
}

