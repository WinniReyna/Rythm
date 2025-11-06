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
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (nearbyInteractable != null && other.GetComponent<IInteractable>() == nearbyInteractable)
        {
            nearbyInteractable = null;
            Debug.Log($"Jugador salió del rango de {other.name}");

            // Aquí podemos cerrar el panel de examineObject
            ExamineObject examineObj = other.GetComponent<ExamineObject>();
            if (examineObj != null)
            {
                examineObj.EndExamine();
            }

            // Si tenías diálogo
            if (dialogueManager != null)
            {
                DialogueManager.Instance.EndDialogue();
            }
        }
    }



}

