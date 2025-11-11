using UnityEngine;
using System;
public enum QuestType
{
    CollectItem,   // Recolectar uno o varios objetos
    DeliverItem,   // Entregar un objeto a un NPC
    TalkToNPC,     // Hablar con un NPC específico
    GoToLocation,  // Llegar a una zona o punto específico
    DefeatEnemy    // (Opcional) Matar un enemigo específico o cantidad
}
