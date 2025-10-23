using UnityEngine;
using Lean.Localization;
using System.Collections;

public class ExamineObject : MonoBehaviour, IInteractable
{
    [Tooltip("Debemos colocar aquí el ScriptableObject que se utilizará. Se encuentra en el mení de Examine/ExamineData.")]
    [SerializeField] private ExamineData data;
    private IExamineUI uiManager;
    private bool isExamining = false;
    private IInputProvider inputProvider;

    // Asignación automática del UI Manager
    private void Awake()
    {
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<ExamineUIManager>();
            inputProvider = new KeyboardInputProvider();
            if (uiManager == null)
            {
                Debug.LogError("ExamineUIManager not found in the scene!");
            }
        }
    }
    private void Update()
    {
        // Si estamos examinando y se presiona barra espaciadora, cerrar panel
        if (isExamining && inputProvider.DialogueLine())
        {
            EndExamine();
        }
    }

    public void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);

        if (uiManager == null || data == null) return;

        // Obtener título y descripción según el idioma actual
        string localizedTitle = data.GetTitle();
        string localizedDescription = data.GetDescription();

        // Mostrar la textura y el texto en la UI
        uiManager.Show(data.objectTexture, localizedTitle + "\n\n" + localizedDescription);

        // Bloquear movimiento del jugador mientras examina
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;

        isExamining = true;
    }

    public void EndExamine()
    {
        if (uiManager != null)
            uiManager.Hide();

        // Esperar antes de habilitar movimiento
        StartCoroutine(ReenableMovement());

        isExamining = false;
    }

    private IEnumerator ReenableMovement()
    {
        // Espera un frame completo o 0.1 segundos
        yield return new WaitForSeconds(0.1f); // 0.1s es suficiente para ignorar el input residual

        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }
}


