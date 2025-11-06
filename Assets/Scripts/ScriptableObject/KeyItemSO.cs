using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyItem", menuName = "Inventory/Key Item")]
public class KeyItemSO : ItemSO, IUsableItem
{
    [Header("ID de la puerta que abre")]
    [Tooltip("Debe coincidir con el RequiredKeyID de la puerta")]
    public string keyID;

    public bool Use(PlayerInteraction player)
    {
        var interactable = player.GetNearbyInteractable();
        if (interactable == null)
        {
            Debug.Log("No hay nada cercano para usar la llave.");
            return false;
        }

        // Revisar si es un Teleporter
        if (interactable is Teleporter teleporter)
        {
            if (teleporter.IsLocked && teleporter.RequiredKeyID == keyID)
            {
                teleporter.Unlock();
                Debug.Log($"Puerta desbloqueada con {itemName}");
                return true; // consumido
            }
            else
            {
                Debug.Log("La llave no encaja aquí.");
                return false;
            }
        }

        Debug.Log("No se puede usar la llave aquí.");
        return false;
    }
}

