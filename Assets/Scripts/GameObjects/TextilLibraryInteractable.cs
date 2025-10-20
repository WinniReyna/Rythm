using UnityEngine;

public class TextilLibraryInteractable : MonoBehaviour, IInteractable
{
    [Header("Referencia a la UI")]
    [SerializeField] private GameObject TextilUIPanel; // Panel con LibraryUI

    public void Interact()
    {
        if (TextilUIPanel == null) return;

        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        if (pauseManager != null)
        {
            // Abre la UI usando el sistema de paneles
            pauseManager.OpenPanel(TextilUIPanel);
        }
        else
        {
            // Fallback por si no hay PauseManager: solo activar el panel
            TextilUIPanel.SetActive(true);
            TextilUI libraryUI = TextilUIPanel.GetComponent<TextilUI>();
            if (libraryUI != null)
                libraryUI.RefreshUI();
        }
    }
}