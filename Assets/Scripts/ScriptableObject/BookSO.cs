using UnityEngine;
using Lean.Localization;

[CreateAssetMenu(fileName = "NewBook", menuName = "Library/BookSO")]
public class BookSO : ScriptableObject
{
    [Header("Identificador único")]
    public string bookID;

    [Header("Visual")]
    public string bookTitle; 

    [Header("Contenido")]
    [TextArea(5, 10)]
    public string storyTextKey; // Clave de Lean Localization para el texto
    public int order; // Para ordenar la lista

    [Header("Opcional")]
    public AudioClip pickupSound;
}
