using UnityEngine;

public class LibraryInteractable : MonoBehaviour, IInteractable
{
    [Header("Referencia a la UI")]
    [SerializeField] private LibraryUI libraryUI;

    public void Interact()
    {
        if (libraryUI == null) return;

        // Activar panel si está cerrado
        if (!libraryUI.gameObject.activeSelf)
            libraryUI.gameObject.SetActive(true);

        // Refrescar la lista de libros
        libraryUI.RefreshUI();
    }
}
