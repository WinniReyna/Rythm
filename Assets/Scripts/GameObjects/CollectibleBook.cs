using UnityEngine;
using Lean.Localization;

public class CollectibleBook : MonoBehaviour, IInteractable
{
    [SerializeField] private BookSO book; // ScriptableObject del libro
    private LibrarySaveLoad library;

    private void Awake()
    {
        library = FindObjectOfType<LibrarySaveLoad>();

        if (library == null) Debug.LogError("LibrarySaveLoad not found!");
        //if (libraryUI == null) Debug.LogError("LibraryUI not found!");
    }

    public void Interact()
    {
        if (book == null) return;

        // Agregar libro a la biblioteca si no está ya
        library.AddBook(book);

        //libraryUI.RefreshUI();


        // Reproducir sonido opcional
        if (book.pickupSound != null)
            AudioSource.PlayClipAtPoint(book.pickupSound, transform.position);

        // Destruir objeto del mundo
        Destroy(gameObject);

        Debug.Log("Libro recogido: " + book.bookTitle);
    }
}
