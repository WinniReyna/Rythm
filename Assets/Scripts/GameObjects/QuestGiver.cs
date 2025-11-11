using UnityEngine;

public class QuestGiver : DialogueObject
{
    [Header("Misión asignada")]
    [SerializeField] private QuestData quest;

    public override void Interact()
    {
        var dialogueManager = DialogueManager.Instance;
        var questManager = QuestManager.Instance;
        if (dialogueManager == null || questManager == null)
            return;

        // Si no hay misión, usa el comportamiento de diálogo base
        if (quest == null)
        {
            base.Interact();
            return;
        }

        // Si la misión ya fue completada
        if (questManager.IsQuestCompleted(quest.questID))
        {
            dialogueManager.StartDialogue(quest.dialogueComplete);
            return;
        }

        // Si la misión es de entregar un ítem
        if (quest.questType == QuestType.DeliverItem)
        {
            if (HasItem(quest.requiredItemID))
            {
                RemoveItem(quest.requiredItemID, 1);
                CompleteQuest(dialogueManager);
            }
            else
            {
                dialogueManager.StartDialogue(quest.dialogueInProgress);
            }
            return;
        }

        // Si la misión es de hablar con un NPC
        if (quest.questType == QuestType.TalkToNPC)
        {
            // Reportamos al QuestManager que el jugador habló con este NPC
            questManager.CheckQuestProgress(quest, dialogueData.npcName);
            dialogueManager.StartDialogue(quest.dialogueStart);
            return;
        }

        // Si la misión es de ir a un lugar, se inicia y luego se completará desde el trigger
        if (quest.questType == QuestType.GoToLocation)
        {
            dialogueManager.StartDialogue(quest.dialogueStart);
            questManager.StartQuest(quest.questID);
            return;
        }

        // Si ya está completada pero no marcada aún
        if (quest.isCompleted)
        {
            CompleteQuest(dialogueManager);
        }
        else
        {
            dialogueManager.StartDialogue(quest.dialogueStart);
            StartQuest();
        }
    }

    private void StartQuest()
    {
        Debug.Log($"Misión iniciada: {quest.questName}");
        QuestManager.Instance.StartQuest(quest.questID);
    }

    private void CompleteQuest(DialogueManager manager)
    {
        manager.StartDialogue(quest.dialogueComplete);
        quest.isCompleted = true;
        QuestManager.Instance.CompleteQuest(quest.questID);

        // Recompensa
        if (quest.givesReward && !string.IsNullOrEmpty(quest.rewardItemID))
        {
            AddItem(quest.rewardItemID, quest.rewardQuantity);
            Debug.Log($"Recompensa añadida: {quest.rewardItemID} x{quest.rewardQuantity}");
        }
    }

    // Métodos conectados al InventoryManager real
    private bool HasItem(string itemID)
    {
        var inv = InventoryManager.Instance;
        if (inv == null) return false;
        return inv.InventorySO.items.Exists(i => i.itemID == itemID && i.quantity > 0);
    }

    private void RemoveItem(string itemID, int amount)
    {
        var inv = InventoryManager.Instance;
        if (inv != null)
            inv.RemoveItem(itemID, amount);
    }

    private void AddItem(string itemID, int amount)
    {
        var inv = InventoryManager.Instance;
        if (inv != null)
            inv.AddItem(itemID, amount);
    }
}
