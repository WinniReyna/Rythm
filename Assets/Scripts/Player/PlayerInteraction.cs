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

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractable = interactable;
            Debug.Log($"Jugador puede interactuar con {other.name}");
        }
    }

    

}

