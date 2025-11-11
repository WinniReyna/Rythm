using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuest", menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    [Header("Identificación")]
    public string questID;
    public string questName;

    [Header("Tipo de misión")]
    public QuestType questType;

    [Header("Condiciones")]
    public string targetNPC;            // Para "TalkToNPC" o "DeliverItem"
    public string targetLocationName;   // Para "GoToLocation"
    public string requiredItemID;       // ID del ítem a entregar

    [Header("Diálogos")]
    public DialogueData dialogueStart;
    public DialogueData dialogueInProgress;
    public DialogueData dialogueComplete;

    [Header("Recompensa")]
    public bool givesReward;
    public string rewardItemID;
    public int rewardQuantity = 1;

    [NonSerialized] public bool isCompleted;
}