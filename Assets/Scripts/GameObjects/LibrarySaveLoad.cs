using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class LibrarySaveLoad : MonoBehaviour
{
    [Header("Todos los libros del juego")]
    [SerializeField] private List<BookSO> allBooks;

    [Header("Libros encontrados")]
    public List<BookSO> foundBooks = new List<BookSO>();

    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "library.json");
        LoadLibrary();
    }

    public void AddBook(BookSO book)
    {
        if (book == null || foundBooks.Contains(book)) return;

        foundBooks.Add(book);
        foundBooks = foundBooks.OrderBy(b => b.order).ToList();
        SaveLibrary();
    }

    public void RemoveBook(BookSO book)
    {
        if (book == null || !foundBooks.Contains(book)) return;

        foundBooks.Remove(book);
        SaveLibrary();
        Debug.Log($"Libro eliminado: {book.bookTitle}");
    }

    public void SaveLibrary()
    {
        LibraryData data = new LibraryData();
        foreach (var book in foundBooks)
        {
            data.foundBookIDs.Add(book.bookID);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Library guardada en: " + savePath);
    }

    public void LoadLibrary()
    {
        foundBooks.Clear();
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        LibraryData data = JsonUtility.FromJson<LibraryData>(json);

        foreach (string id in data.foundBookIDs)
        {
            BookSO book = allBooks.Find(b => b.bookID == id);
            if (book != null)
                foundBooks.Add(book);
        }

        foundBooks = foundBooks.OrderBy(b => b.order).ToList();
        Debug.Log("Library cargada desde: " + savePath);
    }

    public List<BookSO> GetBooks()
    {
        return foundBooks;
    }
}
