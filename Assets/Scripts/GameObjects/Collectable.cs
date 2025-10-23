using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    [Tooltip("Coloca aquí el ItemSO de scriptableObject que creaste para este item.")]
    [SerializeField] private ItemSO itemData;
    [Tooltip("Cantidad de objeto recolectado, puede empezar x1 o x5 o n cantidad de cantidad de item recolectado.")]
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
