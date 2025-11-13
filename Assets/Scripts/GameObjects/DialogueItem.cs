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
            Debug.LogWarning($"{name} no tiene asignado un primer DialogueData.");
            return;
        }

        // Si ya hablamos antes con este NPC mostrar diálogo posterior
        if (!string.IsNullOrEmpty(npcID) &&
            QuestManager.Instance.HasNpcEventCompleted(npcID))
        {
            DialogueManager.Instance.StartDialogue(afterDialogue);
            return;
        }

        // Primera vez hablando
        DialogueManager.Instance.StartDialogue(firstDialogue);

        // Marcar que ya hablamos una vez
        if (!string.IsNullOrEmpty(npcID))
            QuestManager.Instance.MarkNpcEventCompleted(npcID);
    }
}
