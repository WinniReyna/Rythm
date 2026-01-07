using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable nearbyInteractable;
    private DialogueManager dialogueManager;
    private IInputProvider inputProvider;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        inputProvider = new KeyboardInputProvider();
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

            // mostrar UI
            InteractPrompt prompt = other.GetComponent<InteractPrompt>();
            if (prompt != null)
                prompt.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (nearbyInteractable != null && other.GetComponent<IInteractable>() == nearbyInteractable)
        {
            Debug.Log($"Jugador salió del rango de {other.name}");

            // ocultar UI
            InteractPrompt prompt = other.GetComponent<InteractPrompt>();
            if (prompt != null)
                prompt.Hide();

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

