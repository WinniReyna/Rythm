using UnityEngine;

public class DialogueItem : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueData firstDialogue;
    [SerializeField] private DialogueData afterDialogue;
    [SerializeField] private string npcID; 

    public void Interact()
    {
        if (firstDialogue == null)
        {
            Debug.LogWarning($"{name} no tiene asignado un primer DialogueData");
            return;
        }

        if (!string.IsNullOrEmpty(npcID) &&
            QuestManager.Instance.HasNpcEventCompleted(npcID))
        {
            DialogueManager.Instance.StartDialogue(afterDialogue);
            return;
        }

        DialogueManager.Instance.StartDialogue(firstDialogue);

        if (!string.IsNullOrEmpty(npcID))
            QuestManager.Instance.MarkNpcEventCompleted(npcID);
    }
}
