using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCSceneData", menuName = "Dialogue/NPC Scene Data")]
public class NPCSceneData : ScriptableObject
{
    [Header("Información del NPC")]
    public string npcName;

    [Header("Escenas asociadas")]
    [Tooltip("Nombre exacto de la escena de cinemática (debe estar en Build Settings).")]
    public string cinematicSceneName;

    [Tooltip("Nombre exacto de la escena del minijuego (debe estar en Build Settings).")]
    public string minigameSceneName;

    [Header("Configuración opcional")]
    [Tooltip("¿El minijuego otorga un objeto o desbloquea un nuevo diálogo?")]
    public bool grantsReward;

    [Tooltip("Nombre del objeto o evento que se desbloquea después del minijuego.")]
    public string rewardID;
}
