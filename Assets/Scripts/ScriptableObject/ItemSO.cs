using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identificador único")]
    [Tooltip("Puede ser el nombre del item.")]
    public string itemID;

    [Header("Datos visuales")]
    [Tooltip("Nombre que aparecerá en la UI.")]
    public string itemName;
    [Tooltip("Descripción del item.")]
    [TextArea] public string description;
    [Tooltip("Imagen del item.")]
    public Texture2D icon;

    [Header("Instancia para tirar")]
    [Tooltip("El ítem en escena debe ser un prefab para poder instanciarlo nuevamente al sacarlo del inventario.")]
    public GameObject prefab;

    [Header("Opcional")]
    [Tooltip("Si quieren que tenga un sonido el objeto al recoger. Por el momento puede estar vacío.")]
    public AudioClip pickupSound;
}
