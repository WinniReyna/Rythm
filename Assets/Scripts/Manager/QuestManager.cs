using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private string questSavePath;
    private Dictionary<string, QuestState> questStates = new Dictionary<string, QuestState>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        questSavePath = Path.Combine(Application.persistentDataPath, "quests.json");
        LoadQuestData();
    }

    public void CheckQuestProgress(QuestData quest, string npcName = null, string currentLocation = null)
    {
        switch (quest.questType)
        {
            case QuestType.CollectItem:
                if (InventoryManager.Instance.HasItem(quest.requiredItemID))
                    CompleteQuest(quest.questID);
                break;

            case QuestType.TalkToNPC:
                // Se completa si hablas con el NPC correcto
                if (!string.IsNullOrEmpty(npcName) && quest.targetNPC == npcName)
                    CompleteQuest(quest.questID);
                break;

            case QuestType.GoToLocation:
                // Se completa si entras a la ubicación correcta
                if (!string.IsNullOrEmpty(currentLocation) && quest.targetLocationName == currentLocation)
                    CompleteQuest(quest.questID);
                break;

            case QuestType.DeliverItem:
                if (InventoryManager.Instance.HasItem(quest.requiredItemID))
                {
                    InventoryManager.Instance.RemoveItem(quest.requiredItemID);
                    CompleteQuest(quest.questID);
                }
                break;
        }
    }

    public QuestStatus GetQuestStatus(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return QuestStatus.NotStarted;
        return questStates.ContainsKey(questID) ? questStates[questID].status : QuestStatus.NotStarted;
    }

    public void StartQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return;

        if (!questStates.ContainsKey(questID))
            questStates[questID] = new QuestState(questID, QuestStatus.InProgress);
        else if (questStates[questID].status == QuestStatus.NotStarted)
            questStates[questID].status = QuestStatus.InProgress;

        SaveQuestData();
        Debug.Log($"Misión iniciada: {questID}");
    }

    public void CompleteQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return;

        questStates[questID] = new QuestState(questID, QuestStatus.Completed);
        SaveQuestData();

        Debug.Log($"Misión completada: {questID}");
    }

    public void FailQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return;

        questStates[questID] = new QuestState(questID, QuestStatus.Failed);
        SaveQuestData();

        Debug.Log($"Misión fallida: {questID}");
    }

    public void ResetQuest(string questID)
    {
        if (questStates.ContainsKey(questID))
        {
            questStates[questID].status = QuestStatus.NotStarted;
            SaveQuestData();
            Debug.Log($"Misión reiniciada: {questID}");
        }
    }

    [ContextMenu("Reset All Quests")]
    public void ResetAllQuests()
    {
        questStates.Clear();
        if (File.Exists(questSavePath))
            File.Delete(questSavePath);

        Debug.Log("Todos los datos de misiones fueron reiniciados.");
    }

    private void SaveQuestData()
    {
        QuestSaveWrapper wrapper = new QuestSaveWrapper(questStates);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(questSavePath, json);

        Debug.Log($"Misiones guardadas en {questSavePath}");
    }

    private void LoadQuestData()
    {
        if (!File.Exists(questSavePath))
        {
            Debug.Log("No hay archivo de misiones, comenzando vacío.");
            return;
        }

        string json = File.ReadAllText(questSavePath);
        QuestSaveWrapper wrapper = JsonUtility.FromJson<QuestSaveWrapper>(json);

        questStates = wrapper != null ? wrapper.ToDictionary() : new Dictionary<string, QuestState>();
        Debug.Log("Misiones cargadas desde JSON.");
    }

    [System.Serializable]
    private class QuestSaveWrapper
    {
        public List<string> questIDs = new List<string>();
        public List<QuestStatus> statuses = new List<QuestStatus>();

        public QuestSaveWrapper(Dictionary<string, QuestState> dict)
        {
            foreach (var kvp in dict)
            {
                questIDs.Add(kvp.Key);
                statuses.Add(kvp.Value.status);
            }
        }

        public Dictionary<string, QuestState> ToDictionary()
        {
            var dict = new Dictionary<string, QuestState>();
            for (int i = 0; i < questIDs.Count; i++)
            {
                if (i < statuses.Count)
                    dict[questIDs[i]] = new QuestState(questIDs[i], statuses[i]);
            }
            return dict;
        }
    }

    // Métodos auxiliares
    public bool IsQuestCompleted(string questID)
    {
        return GetQuestStatus(questID) == QuestStatus.Completed;
    }

    public bool IsQuestInProgress(string questID)
    {
        return GetQuestStatus(questID) == QuestStatus.InProgress;
    }

    public bool IsQuestNotStarted(string questID)
    {
        return GetQuestStatus(questID) == QuestStatus.NotStarted;
    }
}

