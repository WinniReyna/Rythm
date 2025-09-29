using UnityEngine;

public class DialogueObject : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData dialogueData;

    public void Interact()
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager != null && dialogueData != null)
        {
            manager.ShowDialogue(dialogueData);
        }
    }
}
