using UnityEngine;

[CreateAssetMenu(fileName = "NewChoiceRewardDialogue", menuName = "Dialogue/ChoiceRewardDialogueData")]
public class ChoiceRewardDialogueData : DialogueData
{
    [Header("Opciones de recompensa")]
    public ItemSO[] rewardOptions; // Ítems que el jugador puede elegir
    public bool onlyOnce = true;

    [Header("Diálogo posterior")]
    public DialogueData afterChoiceDialogue; // lo que dirá después de elegir
}

