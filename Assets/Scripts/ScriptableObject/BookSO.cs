using UnityEngine;
using Lean.Localization;

[CreateAssetMenu(fileName = "NewBook", menuName = "Library/BookSO")]
public class BookSO : ScriptableObject
{
    [Header("Identificador �nico")]
    public string bookID;

    [Header("T�tulo del libro")]
    public string titleES;
    public string titleEN;

    [Header("Contenido del libro")]
    [TextArea(5, 10)]
    public string contentES;
    [TextArea(5, 10)]
    public string contentEN;

    [Header("Posici�n en la lista")]
    public int order;

    [Header("Opcional")]
    public AudioClip pickupSound;

    /// <summary>
    /// Obtiene el t�tulo seg�n el idioma actual de Lean Localization
    /// </summary>
    public string GetTitle()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return titleEN;
        return titleES;
    }

    /// <summary>
    /// Obtiene el contenido seg�n el idioma actual de Lean Localization
    /// </summary>
    public string GetContent()
    {
        string lang = LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return contentEN;
        return contentES;
    }
}

