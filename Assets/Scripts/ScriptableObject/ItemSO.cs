using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identificador único")]
    public string itemID;

    [Header("Datos visuales")]
    public string itemName;
    [TextArea] public string description;
    public Texture2D icon;

    [Header("Opcional")]
    public AudioClip pickupSound;
}
