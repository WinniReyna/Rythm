using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Localization;

public class LibraryUI : MonoBehaviour, IMenuPanel
{
    [Header("Referencias UI")]
    [SerializeField] private Transform booksParent;
    [SerializeField] private GameObject bookButtonPrefab;
    [SerializeField] private TMP_Text storyText;

    [Header("Sistema de guardado")]
    [SerializeField] private LibrarySaveLoad librarySaveLoad;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void Open()
    {
        gameObject.SetActive(true);

        // Bloquea movimiento del jugador
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        RefreshUI();
    }

    public void Close()
    {
        gameObject.SetActive(false);

        // Reactiva movimiento del jugador
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;

        // Limpia texto del libro mostrado
        if (storyText != null)
            storyText.text = "";
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

        // Crear botones para cada libro encontrado
        foreach (BookSO book in librarySaveLoad.foundBooks)
        {
            if (book == null) continue;

            GameObject go = Instantiate(bookButtonPrefab, booksParent);

            // Configurar texto del botón
            TMP_Text buttonText = go.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
                buttonText.text = book.GetTitle();

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

            // Botón de eliminar libro (solo en Editor)
            Transform deleteBtnTransform = go.transform.Find("DeleteButton");
            if (deleteBtnTransform != null)
            {
                Button deleteBtn = deleteBtnTransform.GetComponent<Button>();
                if (deleteBtn != null)
                {
#if UNITY_EDITOR
                    deleteBtn.gameObject.SetActive(Application.isPlaying && Application.isEditor);
                    deleteBtn.onClick.RemoveAllListeners();
                    deleteBtn.onClick.AddListener(() => RemoveBook(book));
#else
                    deleteBtn.gameObject.SetActive(false);
#endif
                }
            }
        }
    }

    /// <summary>
    /// Muestra el título y contenido del libro seleccionado
    /// </summary>
    /// <param name="book">Libro a mostrar</param>
    public void ShowBook(BookSO book)
    {
        if (book == null || storyText == null) return;

        // Mostrar título y contenido según idioma actual
        storyText.text = $"<b>{book.GetTitle()}</b>\n\n{book.GetContent()}";
    }

    /// <summary>
    /// Elimina un libro de la biblioteca (solo en Editor)
    /// </summary>
    /// <param name="book">Libro a eliminar</param>
    private void RemoveBook(BookSO book)
    {
#if UNITY_EDITOR
        if (librarySaveLoad != null && book != null)
        {
            librarySaveLoad.RemoveBook(book);
            librarySaveLoad.SaveLibrary();
            RefreshUI();
            Debug.Log($"Libro '{book.GetTitle()}' eliminado de la biblioteca (modo Editor).");
        }
#endif
    }
}
