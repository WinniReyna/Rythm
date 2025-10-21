using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueObject : MonoBehaviour, IInteractable
{
    [Header("Diálogo")]
    [SerializeField] private DialogueData dialogueData;

    [Header("Opcional: escenas y minijuego")]
    [SerializeField] private NPCSceneData sceneData;

    public void Interact()
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager != null && dialogueData != null)
        {
            manager.StartDialogue(dialogueData);

            // Si el NPC tiene escenas asociadas, registrarlas para después del diálogo
            if (sceneData != null)
            {
                // Guardamos en GameState o en el DialogueManager qué hacer al finalizar
                DialogueManager.Instance.SetPostDialogueAction(() =>
                {
                    TriggerSceneData();
                });
            }
        }
    }

    private void TriggerSceneData()
    {
        if (sceneData == null) return;

        // Primero, si hay cinemática
        if (!string.IsNullOrEmpty(sceneData.cinematicSceneName))
        {
            SceneManager.LoadScene(sceneData.cinematicSceneName);
            return;
        }

        // Si no hay cinemática pero sí minijuego
        if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
        {
            SceneManager.LoadScene(sceneData.minigameSceneName);
            return;
        }

        // Aquí podrías manejar recompensas o eventos adicionales
        if (sceneData.grantsReward)
        {
            Debug.Log($"Otorgando recompensa: {sceneData.rewardID}");
        }
    }
}

