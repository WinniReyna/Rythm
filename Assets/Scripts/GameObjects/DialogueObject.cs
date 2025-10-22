using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DialogueObject : MonoBehaviour, IInteractable
{
    [Header("Di�logo")]
    [SerializeField] private DialogueData dialogueData;

    [Header("Opcional: escenas y minijuego")]
    [SerializeField] private NPCSceneData sceneData;

    public DialogueData DialogueData => dialogueData;

    public void Interact()
    {
        var manager = DialogueManager.Instance;
        if (manager != null && dialogueData != null)
        {
            manager.StartDialogue(dialogueData);

            if (sceneData != null)
            {
                manager.SetPostDialogueAction(() =>
                {
                    var player = PlayerMovement.Instance != null ? PlayerMovement.Instance.transform : null;
                    string npcName = dialogueData != null ? dialogueData.npcName : gameObject.name;
                    GameState.Instance.TriggerScene(sceneData, player, npcName);
                });
            }
        }
    }

    /// <summary>
    /// Cambiar a cinem�tica o minijuego usando GameState
    /// </summary>
    private void TriggerSceneData()
    {
        if (sceneData == null)
            return;

        // Usamos GameState si existe
        if (GameState.Instance != null)
        {
            Transform playerTransform = PlayerMovement.Instance != null ? PlayerMovement.Instance.transform : null;
            string npcName = dialogueData != null ? dialogueData.npcName : gameObject.name;

            GameState.Instance.TriggerScene(sceneData, playerTransform, npcName);
        }
        else
        {
            Debug.LogWarning("No existe GameState en la escena. Cargando escena directamente.");

            if (!string.IsNullOrEmpty(sceneData.cinematicSceneName))
                SceneManager.LoadScene(sceneData.cinematicSceneName);
            else if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
                SceneManager.LoadScene(sceneData.minigameSceneName);
        }

        // Opcional: manejar recompensas o eventos
        if (sceneData.grantsReward)
        {
            Debug.Log($"Otorgando recompensa: {sceneData.rewardID}");
            // Aqu� podr�as llamar a tu sistema de inventario o logros
        }
    }
}


