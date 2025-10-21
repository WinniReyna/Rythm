using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCSceneData", menuName = "Dialogue/NPC Scene Data")]
public class NPCSceneData : ScriptableObject
{
    [Header("Informaci�n del NPC")]
    public string npcName;

    [Header("Escenas asociadas")]
    [Tooltip("Nombre exacto de la escena de cinem�tica (debe estar en Build Settings).")]
    public string cinematicSceneName;

    [Tooltip("Nombre exacto de la escena del minijuego (debe estar en Build Settings).")]
    public string minigameSceneName;

    [Header("Configuraci�n opcional")]
    [Tooltip("�El minijuego otorga un objeto o desbloquea un nuevo di�logo?")]
    public bool grantsReward;

    [Tooltip("Nombre del objeto o evento que se desbloquea despu�s del minijuego.")]
    public string rewardID;
}
