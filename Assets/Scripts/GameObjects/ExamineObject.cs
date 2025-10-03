using UnityEngine;
using Lean.Localization;

public class ExamineObject : MonoBehaviour, IInteractable
{
    [SerializeField] private ExamineData data;
    private IExamineUI uiManager;

    // Asignación automática del UI Manager
    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<ExamineUIManager>();
            if (uiManager == null)
            {
                Debug.LogError("ExamineUIManager not found in the scene!");
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);

        if (uiManager == null || data == null) return;

        // Obtener texto traducido usando Lean Localization
        string localizedText = LeanLocalization.GetTranslationText(data.localizationKey);

        // Mostrar la textura y el texto en la UI
        uiManager.Show(data.objectTexture, localizedText);
    }

    public void EndExamine()
    {
        if (uiManager != null)
            uiManager.Hide();
    }
}


