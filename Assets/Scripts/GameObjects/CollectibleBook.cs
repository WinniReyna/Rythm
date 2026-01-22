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

        library.LoadLibrary();

        if (library.foundBooks.Contains(book)) gameObject.SetActive(false);
        
    }

    public void Interact()
    {
        if (book == null || library == null) return;

        if (!library.foundBooks.Contains(book))
        {
            library.AddBook(book);

            if (book.pickupSound != null)
                AudioSource.PlayClipAtPoint(book.pickupSound, transform.position);

            Destroy(gameObject);
        }
        else
        {
            //Debug.Log($" El libro '{book.bookTitle}' ya fue recogido.");
        }
    }
}
