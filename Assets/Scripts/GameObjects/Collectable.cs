using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int quantity = 1;

    public void Interact()
    {
        if (itemData != null)
        {
            // Pasar solo el ID al inventario
            InventoryManager.Instance.AddItem(itemData.itemID, quantity);

            Debug.Log($"Se agregó {itemData.itemName} x{quantity} al inventario");

            if (itemData.pickupSound != null)
                AudioSource.PlayClipAtPoint(itemData.pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}