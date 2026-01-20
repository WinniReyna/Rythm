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

        if (interactable is Teleporter teleporter)
        {
            if (teleporter.IsLocked && teleporter.RequiredKeyID == keyID)
            {                
                PauseManager.Instance?.CloseCurrentPanel();

                teleporter.Unlock(player.gameObject);

                Debug.Log($"Puerta desbloqueada con {GetItemName()}");
                return true;
            }
        }

        Debug.Log("No se puede usar la llave aqui.");
        return false;
    }


}

