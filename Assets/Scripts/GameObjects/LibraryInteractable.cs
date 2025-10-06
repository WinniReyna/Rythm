using UnityEngine;

public class LibraryInteractable : MonoBehaviour, IInteractable
{
    [Header("Referencia a la UI")]
    [SerializeField] private GameObject libraryUIPanel; // Panel con LibraryUI

    public void Interact()
    {
        if (libraryUIPanel == null) return;

        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        if (pauseManager != null)
        {
            // Abre la UI usando el sistema de paneles
            pauseManager.OpenPanel(libraryUIPanel);
        }
        else
        {
            // Fallback por si no hay PauseManager: solo activar el panel
            libraryUIPanel.SetActive(true);
            LibraryUI libraryUI = libraryUIPanel.GetComponent<LibraryUI>();
            if (libraryUI != null)
                libraryUI.RefreshUI();
        }
    }
}
