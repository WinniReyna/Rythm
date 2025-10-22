using UnityEngine;

public class ReturnPointHandler : MonoBehaviour
{
    private void Start()
    {
        if (GameState.Instance != null && GameState.Instance.returningFromEvent)
        {
            // Restaurar posici�n del jugador
            var player = PlayerMovement.Instance;
            if (player != null)
            {
                player.transform.position = GameState.Instance.playerPosition;
                player.canMove = true;
            }

            // Reanudar di�logo con el NPC correcto
            var allNpcs = FindObjectsOfType<DialogueObject>();
            foreach (var npc in allNpcs)
            {
                if (npc.DialogueData != null && npc.DialogueData.npcName == GameState.Instance.lastNPCName)
                {
                    DialogueManager.Instance.StartDialogue(npc.DialogueData);
                    break;
                }
            }

            // Limpiar bandera para que no vuelva a ejecutarse
            GameState.Instance.returningFromEvent = false;
        }
    }
}

