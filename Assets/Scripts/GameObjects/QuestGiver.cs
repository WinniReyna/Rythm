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

        // Si no hay misión, usamos el diálogo base
        if (quest == null)
        {
            base.Interact();
            return;
        }

        // Estado actual de la misión
        var status = questManager.GetQuestStatus(quest.questID);

        switch (status)
        {
            // Misión completada
            case QuestStatus.Completed:
                dialogueManager.StartDialogue(quest.dialogueComplete);
                return;

            // Misión en progreso
            case QuestStatus.InProgress:
                if (quest.questType == QuestType.DeliverItem)
                {
                    HandleDeliverItemQuest(); // Se maneja por separado
                    return;
                }
                dialogueManager.StartDialogue(quest.dialogueInProgress);
                return;

            // Misión no iniciada
            case QuestStatus.NotStarted:
                HandleQuestStart(dialogueManager, questManager);
                return;
        }
    }

    // Inicio de misión según tipo
    private void HandleQuestStart(DialogueManager dialogueManager, QuestManager questManager)
    {
        switch (quest.questType)
        {
            case QuestType.DeliverItem:
                HandleDeliverItemQuest(startingQuest: true);
                break;

            case QuestType.CollectItem:
            case QuestType.GoToLocation:
            case QuestType.TalkToNPC:
                dialogueManager.StartDialogue(quest.dialogueStart);
                questManager.StartQuest(quest.questID);
                Debug.Log($"Misión '{quest.questName}' iniciada correctamente.");
                break;

            default:
                dialogueManager.StartDialogue(quest.dialogueStart);
                break;
        }
    }

    // Lógica separada para misiones de entrega
    private void HandleDeliverItemQuest(bool startingQuest = false)
    {
        var dialogueManager = DialogueManager.Instance;
        var questManager = QuestManager.Instance;
        var inv = InventoryManager.Instance;

        if (inv == null || dialogueManager == null || questManager == null)
            return;

        bool hasItem = HasItem(quest.requiredItemID);

        // Caso 1: Ya tiene el ítem entregar y completar
        if (hasItem)
        {
            RemoveItem(quest.requiredItemID, 1);
            CompleteQuest(dialogueManager);
            Debug.Log($"Misión '{quest.questName}' completada. Se entregó {quest.requiredItemID}.");
        }
        // Caso 2: No lo tiene y recién empieza
        else if (startingQuest)
        {
            dialogueManager.StartDialogue(quest.dialogueStart);
            questManager.StartQuest(quest.questID);
            Debug.Log($"Misión '{quest.questName}' iniciada. Se requiere {quest.requiredItemID}.");
        }
        // Caso 3: No lo tiene y ya estaba en progreso
        else
        {
            dialogueManager.StartDialogue(quest.dialogueInProgress);
            Debug.Log($"Aún no tienes el ítem '{quest.requiredItemID}' para la misión '{quest.questName}'.");
        }
    }

    // Completar misión y dar recompensa
    private void CompleteQuest(DialogueManager manager)
    {
        QuestManager.Instance.CompleteQuest(quest.questID);
        quest.isCompleted = true;
        manager.StartDialogue(quest.dialogueComplete);

        // Recompensa
        if (quest.givesReward && !string.IsNullOrEmpty(quest.rewardItemID))
        {
            AddItem(quest.rewardItemID, quest.rewardQuantity);
            Debug.Log($"Recompensa obtenida: {quest.rewardItemID} x{quest.rewardQuantity}");
        }
    }

    // Métodos auxiliares para inventario
    private bool HasItem(string itemID)
    {
        var inv = InventoryManager.Instance;
        if (inv == null) return false;
        return inv.InventorySO.items.Exists(i => i.itemID == itemID && i.quantity > 0);
    }

    private void RemoveItem(string itemID, int amount)
    {
        var inv = InventoryManager.Instance;
        inv?.RemoveItem(itemID, amount);
    }

    private void AddItem(string itemID, int amount)
    {
        var inv = InventoryManager.Instance;
        inv?.AddItem(itemID, amount);
    }
}
