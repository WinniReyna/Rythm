[System.Serializable]
public class QuestState
{
    public string questID;
    public QuestStatus status;

    public QuestState(string id, QuestStatus st)
    {
        questID = id;
        status = st;
    }
}

