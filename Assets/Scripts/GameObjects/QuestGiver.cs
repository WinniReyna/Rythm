using UnityEngine;

public class QuestGiver : DialogueObject
{
    [Header("Misión asignada")]
    [SerializeField] private QuestData quest;

    public override void Interact()
    {
        var manager = DialogueManager.Instance;
        if (manager == null)
            return;

        // Si no hay misión, comportamiento normal de diálogo
        if (quest == null)
        {
            base.Interact();
            return;
        }

        // Ya completada
        if (QuestManager.Instance.IsQuestCompleted(quest.questID))
        {
            manager.StartDialogue(quest.dialogueComplete);
            return;
        }

        // --- MISIÓN DE ENTREGAR ITEM ---
        if (quest.questType == QuestType.DeliverItem)
        {
            if (HasItem(quest.requiredItemID))
            {
                RemoveItem(quest.requiredItemID, 1);
                CompleteQuest(manager);
            }
            else
            {
                manager.StartDialogue(quest.dialogueInProgress);
            }
            return;
        }

        // --- MISIÓN DE IR O HABLAR ---
        if (quest.isCompleted)
        {
            CompleteQuest(manager);
        }
        else
        {
            manager.StartDialogue(quest.dialogueStart);
            StartQuest();
        }
    }

    private void StartQuest()
    {
        Debug.Log($"Misión iniciada: {quest.questName}");
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
