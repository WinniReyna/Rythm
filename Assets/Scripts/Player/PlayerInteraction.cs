using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable nearbyInteractable;
    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (nearbyInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            nearbyInteractable.Interact();
        }
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

            //Aquí hacemos desaparecer el texto
            if (dialogueManager != null)
            {
                dialogueManager.HideDialogue();
            }
        }
    }
}

