using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyItem", menuName = "Inventory/Key Item")]
public class KeyItemSO : ItemSO, IUsableItem
{
    [Header("ID de la puerta que abre")]
    public string doorID;

    public void Use(PlayerInteraction user)
    {
        var interaction = user.GetComponent<PlayerInteraction>();
        if (interaction == null)
        {
            Debug.Log("No se encontró PlayerInteraction en el jugador.");
            return;
        }

        var interactable = interaction.GetNearbyInteractable();
        if (interactable == null)
        {
            Debug.Log("No hay nada cercano para usar la llave.");
            return;
        }

        // Revisamos si es un Teleporter bloqueado
        if (interactable is Teleporter teleporter)
        {
            if (teleporter.IsLocked && teleporter.RequiredKeyID == doorID)
            {
                teleporter.Unlock();
                Debug.Log($"Puerta desbloqueada con {itemName}");
            }
            else
            {
                Debug.Log("La llave no coincide o la puerta ya está abierta.");
            }
        }
        else
        {
            Debug.Log("Este objeto no puede desbloquearse con una llave.");
        }
    }
}
