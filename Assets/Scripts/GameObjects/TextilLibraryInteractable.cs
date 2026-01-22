using UnityEngine;

public class TextilLibraryInteractable : MonoBehaviour, IInteractable
{
    [Header("Referencia a la UI")]
    [SerializeField] private GameObject TextilUIPanel; 

    public void Interact()
    {
        if (TextilUIPanel == null) return;

        PauseManager pauseManager = FindObjectOfType<PauseManager>();
        if (pauseManager != null)
        {
            pauseManager.OpenPanel(TextilUIPanel);
        }
        else
        {
            TextilUIPanel.SetActive(true);
            TextilUI libraryUI = TextilUIPanel.GetComponent<TextilUI>();
            if (libraryUI != null)
                libraryUI.RefreshUI();
        }
    }
}