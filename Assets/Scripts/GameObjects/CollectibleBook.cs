using UnityEngine;

public class CollectibleBook : MonoBehaviour, IInteractable
{
    [SerializeField] private BookSO book;
    private LibrarySaveLoad library;

    private void Awake()
    {
        library = FindObjectOfType<LibrarySaveLoad>();

        if (library == null)
        {
            Debug.LogError(" LibrarySaveLoad not found!");
        }
    }

    private void Start()
    {
        if (library == null || book == null) return;

        // Esperar a que los datos se carguen
        library.LoadLibrary();

        // Si ya existe el libro en la lista, lo ocultamos
        if (library.foundBooks.Contains(book))
        {
            Debug.Log($" Libro '{book.bookTitle}' ya fue recolectado. Ocultando objeto...");
            gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (book == null || library == null) return;

        // Agregar libro solo si no está en la lista
        if (!library.foundBooks.Contains(book))
        {
            library.AddBook(book);

            if (book.pickupSound != null)
                AudioSource.PlayClipAtPoint(book.pickupSound, transform.position);

            Destroy(gameObject);
            Debug.Log($" Libro recogido: {book.bookTitle}");
        }
        else
        {
            Debug.Log($" El libro '{book.bookTitle}' ya fue recogido.");
        }
    }
}
