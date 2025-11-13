using UnityEngine;

public class DialogueItem : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData dialogueData;

    public void Interact()
    {
        if (dialogueData == null)
        {
            Debug.LogWarning($"{name} no tiene asignado un DialogueData.");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueData);
    }
}
