using UnityEngine;

public class DialogueObject : MonoBehaviour, IInteractable
{
    private DialogueData dialogueData;

    public void Interact()
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager != null && dialogueData != null)
        {
            manager.StartDialogue(dialogueData);
        }
    }
}
