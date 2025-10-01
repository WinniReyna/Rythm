using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    public ItemSO itemData;
    public int quantity = 1;

    public void Interact()
    {
        if (itemData != null)
        {
            InventoryManager.Instance.AddItem(itemData.itemID, quantity);
            Destroy(gameObject);
        }
    }
}
