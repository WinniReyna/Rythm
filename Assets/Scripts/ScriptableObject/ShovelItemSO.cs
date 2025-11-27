using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Shovel")]
public class ShovelItemSO : ItemSO, IUsableItem
{
    [Header("Tiempo excavación")]
    public float digDuration = 1.5f;

    public bool Use(PlayerInteraction player)
    {
        var interactable = player.GetNearbyInteractable();
        if (interactable == null)
        {
            Debug.Log("No hay nada cercano para cavar.");
            return false; // no se consume
        }

        if (interactable is DiggableSpot diggable)
        {
            diggable.Dig(digDuration);
            Debug.Log($"Usaste {GetItemName()} para cavar aquí.");
            return false; // No se consume la pala
        }

        Debug.Log("No se puede cavar en este lugar.");
        return false;
    }
}
