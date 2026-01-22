using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DialogueObject : MonoBehaviour, IInteractable
{
    [Header("Diálogo")]
    [SerializeField] protected DialogueData dialogueData;

    [Header("Opcional escenas y minijuego")]
    [SerializeField] private NPCSceneData sceneData;

    public DialogueData DialogueData => dialogueData;

    public virtual void Interact()
    {
        var manager = DialogueManager.Instance;
        if (manager == null || dialogueData == null)
            return;

        manager.OnDialogueEnded = null;
        manager.StartDialogue(dialogueData);

        if (sceneData != null)
        {
            manager.OnDialogueEnded += () =>
            {
                var returnPointHandler = FindObjectOfType<ReturnPointHandler>();
                if (returnPointHandler != null)
                {
                    returnPointHandler.SaveGameState();
                    Debug.Log("Estado del juego guardado antes de cambiar de escena");
                }

                var player = PlayerMovement.Instance != null ? PlayerMovement.Instance.transform : null;
                string npcName = dialogueData != null ? dialogueData.GetNpcName() : gameObject.name;

                GameState.Instance.TriggerScene(sceneData, player, npcName);
            };
        }
    }

    private void TriggerSceneData()
    {
        if (sceneData == null)
            return;

        if (GameState.Instance != null)
        {
            Transform playerTransform = PlayerMovement.Instance != null ? PlayerMovement.Instance.transform : null;
            string npcName = dialogueData != null ? dialogueData.GetNpcName() : gameObject.name;

            GameState.Instance.TriggerScene(sceneData, playerTransform, npcName);
        }
        else
        {
            Debug.LogWarning("No existe GameState en la escena. Cargando escena directamente");

            if (!string.IsNullOrEmpty(sceneData.cinematicSceneName))
                SceneManager.LoadScene(sceneData.cinematicSceneName);
            else if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
                SceneManager.LoadScene(sceneData.minigameSceneName);
        }

        if (sceneData.grantsReward)
            Debug.Log($"Otorgando recompensa: {sceneData.rewardID}");
        
    }
}



