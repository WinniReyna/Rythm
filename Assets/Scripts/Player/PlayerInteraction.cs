using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable nearbyInteractable;
    private DialogueManager dialogueManager;
    private IInputProvider inputProvider;

    private ReturnPointHandler returnPointHandler;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        returnPointHandler = FindObjectOfType<ReturnPointHandler>();
        inputProvider = new KeyboardInputProvider();

        if (returnPointHandler == null)
            Debug.LogWarning("No se encontró ReturnPointHandler. El juego no se guardará automáticamente al usar/dropear/borrar ítems.");
    }

    void Update()
    {
        if (nearbyInteractable != null && inputProvider != null && inputProvider.InteractPressed())
        {
            nearbyInteractable.Interact();
        }
    }
    public IInteractable GetNearbyInteractable()
    {
        return nearbyInteractable;
    }


    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractable = interactable;
            Debug.Log($"Jugador puede interactuar con {other.name}");

            returnPointHandler?.SaveGameState();

            //Mostrar outliner
            SpriteOutline outline = other.GetComponentInChildren<SpriteOutline>();
            if (outline != null)
                outline.EnableOutline(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearbyInteractable != null && other.GetComponent<IInteractable>() == nearbyInteractable)
        {
            Debug.Log($"Jugador salió del rango de {other.name}");

            //Ocultar outliner
            SpriteOutline outline = other.GetComponentInChildren<SpriteOutline>();
            if (outline != null)
                outline.EnableOutline(false);

            nearbyInteractable = null;

            // Cerrar examine
            ExamineObject examineObj = other.GetComponent<ExamineObject>();
            if (examineObj != null)
                examineObj.EndExamine();

            // Cerrar diálogo
            if (dialogueManager != null)
                DialogueManager.Instance.EndDialogue();
        }
    }
}

