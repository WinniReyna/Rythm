using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIItem : MonoBehaviour
{
    [SerializeField] private RawImage icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    public void Setup(ItemSO itemData, int quantity)
    {
        if (itemData == null) return;

        // Icono
        icon.texture = itemData.icon;

        // Nombre con cantidad
        nameText.text = itemData.itemName + (quantity > 1 ? $" x{quantity}" : "");

        // Descripción
        descriptionText.text = itemData.description;
    }
}
