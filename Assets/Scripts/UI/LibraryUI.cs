using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;

public class LibraryUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public Transform booksParent;          // Panel donde se instanciarán los botones
    public GameObject bookButtonPrefab;    // Prefab de cada botón de libro
    public TMP_Text storyText;             // Texto donde se mostrará la historia

    [Header("Sistema de guardado")]
    public LibrarySaveLoad librarySaveLoad;

    private void OnEnable()
    {
        RefreshUI();
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

        // Limpiar botones antiguos
        foreach (Transform child in booksParent)
            Destroy(child.gameObject);

        // Crear un botón por cada libro encontrado
        foreach (BookSO book in librarySaveLoad.foundBooks)
        {
            if (book == null) continue;

            GameObject go = Instantiate(bookButtonPrefab, booksParent);

            // Asignar texto del botón principal (puede ser título del libro)
            TMP_Text buttonText = go.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
                buttonText.text = book.bookTitle;

            // Botón para leer el libro
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

            // Botón para eliminar el libro
            Transform deleteBtnTransform = go.transform.Find("DeleteButton");
            if (deleteBtnTransform != null)
            {
                Button deleteBtn = deleteBtnTransform.GetComponent<Button>();
                if (deleteBtn != null)
                {
#if UNITY_EDITOR
                    deleteBtn.gameObject.SetActive(Application.isPlaying && Application.isEditor);
#else
                    deleteBtn.gameObject.SetActive(false);
#endif
                    deleteBtn.onClick.RemoveAllListeners();
                    deleteBtn.onClick.AddListener(() => RemoveBook(book));
                }
            }
        }
    }

    // Mostrar la descripción del libro usando Lean Localization
    public void ShowBook(BookSO book)
    {
        if (book == null) return;

        string localizedText = LeanLocalization.GetTranslationText(book.storyTextKey);
        storyText.text = localizedText;
    }

    // Eliminar libro de la biblioteca (solo en Editor)
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
