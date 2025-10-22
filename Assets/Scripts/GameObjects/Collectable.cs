using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private int quantity = 1;

    public void Interact()
    {
        if (itemData != null)
        {
            InventoryManager.Instance.AddItem(itemData.itemID, quantity);
            Destroy(gameObject);
        }
    }
}
