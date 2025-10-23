using UnityEngine;
using Lean.Localization;

[CreateAssetMenu(fileName = "NewBook", menuName = "Library/BookSO")]
public class BookSO : ScriptableObject
{
    [Header("Identificador único")]
    public string bookID;

    [Header("Título del libro")]
    public string titleES;
    public string titleEN;

    [Header("Contenido del libro")]
    [TextArea(5, 10)]
    public string contentES;
    [TextArea(5, 10)]
    public string contentEN;

    [Header("Posición en la lista")]
    public int order;

    [Header("Opcional")]
    public AudioClip pickupSound;

    /// <summary>
    /// Obtiene el título según el idioma actual de Lean Localization
    /// </summary>
    public string GetTitle()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return titleEN;
        return titleES;
    }

    /// <summary>
    /// Obtiene el contenido según el idioma actual de Lean Localization
    /// </summary>
    public string GetContent()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return contentEN;
        return contentES;
    }
}

