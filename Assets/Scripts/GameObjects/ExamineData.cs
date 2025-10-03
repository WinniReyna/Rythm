using UnityEngine;

[CreateAssetMenu(fileName = "ExamineData", menuName = "Examine/Examine Data")]
public class ExamineData : ScriptableObject
{
    public Texture2D objectTexture;    // Textura a mostrar en RawImage
    public string localizationKey;      // Clave para el texto localizado
}
