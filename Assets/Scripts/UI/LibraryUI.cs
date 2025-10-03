using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;
using System.Collections.Generic;

public class LibraryUI : MonoBehaviour
{
    public Transform booksParent;
    public GameObject bookButtonPrefab;

    public TMP_Text storyText;

    public LibrarySaveLoad librarySaveLoad;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        // Limpiar botones
        foreach (Transform child in booksParent)
            Destroy(child.gameObject);

        // Crear botones
        foreach (BookSO book in librarySaveLoad.foundBooks)
        {
            GameObject go = Instantiate(bookButtonPrefab, booksParent);
            TMP_Text buttonText = go.GetComponentInChildren<TMP_Text>();
            buttonText.text = book.bookTitle;

            Button btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowBook(book));
        }
    }

    public void ShowBook(BookSO book)
    {
        if (book == null) return;

        storyText.text = LeanLocalization.GetTranslationText(book.storyTextKey);
    }
}
