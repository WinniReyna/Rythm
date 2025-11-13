using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private string questSavePath;
    private Dictionary<string, QuestState> questStates = new Dictionary<string, QuestState>();

    private Dictionary<string, bool> npcDialogueStates = new Dictionary<string, bool>();

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
                if (!string.IsNullOrEmpty(npcName) && quest.targetNPC == npcName)
                    CompleteQuest(quest.questID);
                break;

            case QuestType.GoToLocation:
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
    }

    public void CompleteQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return;

        questStates[questID] = new QuestState(questID, QuestStatus.Completed);
        SaveQuestData();
    }

    public void FailQuest(string questID)
    {
        if (string.IsNullOrEmpty(questID)) return;

        questStates[questID] = new QuestState(questID, QuestStatus.Failed);
        SaveQuestData();
    }

    public void ResetQuest(string questID)
    {
        if (questStates.ContainsKey(questID))
        {
            questStates[questID].status = QuestStatus.NotStarted;
            SaveQuestData();
        }
    }

    [ContextMenu("Reset All Quests")]
    public void ResetAllQuests()
    {
        questStates.Clear();
        npcDialogueStates.Clear();
        if (File.Exists(questSavePath))
            File.Delete(questSavePath);
    }

    private void SaveQuestData()
    {
        QuestSaveWrapper wrapper = new QuestSaveWrapper(questStates, npcDialogueStates);
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(questSavePath, json);
    }

    private void LoadQuestData()
    {
        if (!File.Exists(questSavePath))
            return;

        string json = File.ReadAllText(questSavePath);
        QuestSaveWrapper wrapper = JsonUtility.FromJson<QuestSaveWrapper>(json);

        questStates = wrapper != null ? wrapper.ToQuestDictionary() : new Dictionary<string, QuestState>();
        npcDialogueStates = wrapper != null ? wrapper.ToNpcDictionary() : new Dictionary<string, bool>();
    }

    public bool HasNpcEventCompleted(string npcID)
    {
        return npcDialogueStates.ContainsKey(npcID) && npcDialogueStates[npcID];
    }

    public void MarkNpcEventCompleted(string npcID)
    {
        if (string.IsNullOrEmpty(npcID)) return;
        npcDialogueStates[npcID] = true;
        SaveQuestData();
    }

    [System.Serializable]
    private class QuestSaveWrapper
    {
        public List<string> questIDs = new List<string>();
        public List<QuestStatus> statuses = new List<QuestStatus>();
        public List<string> npcIDs = new List<string>();
        public List<bool> npcCompleted = new List<bool>();

        public QuestSaveWrapper(Dictionary<string, QuestState> questDict, Dictionary<string, bool> npcDict)
        {
            foreach (var kvp in questDict)
            {
                questIDs.Add(kvp.Key);
                statuses.Add(kvp.Value.status);
            }

            foreach (var kvp in npcDict)
            {
                npcIDs.Add(kvp.Key);
                npcCompleted.Add(kvp.Value);
            }
        }

        public Dictionary<string, QuestState> ToQuestDictionary()
        {
            var dict = new Dictionary<string, QuestState>();
            for (int i = 0; i < questIDs.Count; i++)
                if (i < statuses.Count)
                    dict[questIDs[i]] = new QuestState(questIDs[i], statuses[i]);
            return dict;
        }

        public Dictionary<string, bool> ToNpcDictionary()
        {
            var dict = new Dictionary<string, bool>();
            for (int i = 0; i < npcIDs.Count; i++)
                if (i < npcCompleted.Count)
                    dict[npcIDs[i]] = npcCompleted[i];
            return dict;
        }
    }
}


