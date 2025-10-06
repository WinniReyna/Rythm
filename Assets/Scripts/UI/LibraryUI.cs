using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;

public class LibraryUI : MonoBehaviour, IMenuPanel
{
    [Header("Referencias UI")]
    public Transform booksParent;
    public GameObject bookButtonPrefab;
    public TMP_Text storyText;

    [Header("Sistema de guardado")]
    public LibrarySaveLoad librarySaveLoad;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        RefreshUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void RefreshUI()
    {
        if (librarySaveLoad == null)
        {
            Debug.LogWarning("LibrarySaveLoad no asignado!");
            return;
        }

        if (booksParent == null || bookButtonPrefab == null)
        {
            Debug.LogWarning("BooksParent o BookButtonPrefab no asignado!");
            return;
        }

        foreach (Transform child in booksParent)
            Destroy(child.gameObject);

        foreach (BookSO book in librarySaveLoad.foundBooks)
        {
            if (book == null) continue;

            GameObject go = Instantiate(bookButtonPrefab, booksParent);

            TMP_Text buttonText = go.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
                buttonText.text = book.bookTitle;

            // Botón de leer libro
            Transform readBtnTransform = go.transform.Find("ReadButton");
            if (readBtnTransform != null)
            {
                Button readBtn = readBtnTransform.GetComponent<Button>();
                if (readBtn != null)
                {
                    readBtn.onClick.RemoveAllListeners();
                    readBtn.onClick.AddListener(() => ShowBook(book));
                }
            }

            // Botón de eliminar libro
            Transform deleteBtnTransform = go.transform.Find("DeleteButton");
            if (deleteBtnTransform != null)
            {
                Button deleteBtn = deleteBtnTransform.GetComponent<Button>();
#if UNITY_EDITOR
                deleteBtn.gameObject.SetActive(Application.isPlaying && Application.isEditor);
#else
                deleteBtn.gameObject.SetActive(false);
#endif
                if (deleteBtn != null)
                {
                    deleteBtn.onClick.RemoveAllListeners();
                    deleteBtn.onClick.AddListener(() => RemoveBook(book));
                }
            }
        }
    }

    public void ShowBook(BookSO book)
    {
        if (book == null) return;
        storyText.text = LeanLocalization.GetTranslationText(book.storyTextKey);
    }

    private void RemoveBook(BookSO book)
    {
#if UNITY_EDITOR
        if (librarySaveLoad != null && book != null)
        {
            librarySaveLoad.RemoveBook(book);
            librarySaveLoad.SaveLibrary();
            RefreshUI();
            Debug.Log($"Libro '{book.bookTitle}' eliminado de la biblioteca (modo Editor).");
        }
#endif
    }
}

