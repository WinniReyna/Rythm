using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueObject : MonoBehaviour, IInteractable
{
    [Header("Di�logo")]
    [SerializeField] private DialogueData dialogueData;

    [Header("Opcional: escenas y minijuego")]
    [SerializeField] private NPCSceneData sceneData;

    public void Interact()
    {
        DialogueManager manager = FindObjectOfType<DialogueManager>();
        if (manager != null && dialogueData != null)
        {
            manager.StartDialogue(dialogueData);

            // Si el NPC tiene escenas asociadas, registrarlas para despu�s del di�logo
            if (sceneData != null)
            {
                // Guardamos en GameState o en el DialogueManager qu� hacer al finalizar
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

        // Primero, si hay cinem�tica
        if (!string.IsNullOrEmpty(sceneData.cinematicSceneName))
        {
            SceneManager.LoadScene(sceneData.cinematicSceneName);
            return;
        }

        // Si no hay cinem�tica pero s� minijuego
        if (!string.IsNullOrEmpty(sceneData.minigameSceneName))
        {
            SceneManager.LoadScene(sceneData.minigameSceneName);
            return;
        }

        // Aqu� podr�as manejar recompensas o eventos adicionales
        if (sceneData.grantsReward)
        {
            Debug.Log($"Otorgando recompensa: {sceneData.rewardID}");
        }
    }
}

