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

    [Header("Instancia para tirar")]
    public GameObject prefab;

    [Header("Opcional")]
    public AudioClip pickupSound;
}
