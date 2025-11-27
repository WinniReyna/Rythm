using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewTextil", menuName = "Inventory/Textil Item")]
public class TextilSO : ItemSO, IUsableItem
{
    [Header("Escena a cargar")]
    [Tooltip("Nombre de la escena a cargar cuando se use el objeto.")]
    public string targetSceneName;

    public bool Use(PlayerInteraction player)
    {
        var interactable = player.GetNearbyInteractable();
        if (interactable == null)
        {
            Debug.Log("No hay ningún objeto cercano con el que interactuar.");
            return false;
        }

        // Verificar si es un Telar
        if (interactable is Telar)
        {
            if (!string.IsNullOrEmpty(targetSceneName))
            {
                Debug.Log($"Usando {GetItemName()} en el telar, cambiando a escena {targetSceneName}");
                SceneManager.LoadScene(targetSceneName);
                return true; // objeto consumido
            }
            else
            {
                Debug.LogWarning("No se definió la escena de destino en el TextilSO.");
                return false;
            }
        }

        Debug.Log("Este objeto solo puede usarse en un telar.");
        return false;
    }
}
