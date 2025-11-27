using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    [Header("Identificador único")]
    [Tooltip("Puede ser el nombre del item.")]
    public string itemID;

    [Header("Description ES")]
    public string itemNameES;
    [TextArea] public string descriptionES;

    [Header("Description EN")]
    public string itemNameEN;
    [TextArea] public string descriptionEN;

    [Header("Imagen del item")]
    public Texture2D icon;

    [Header("Instancia para tirar")]
    public GameObject prefab;

    [Header("Audios")]
    public AudioClip pickupSound;
    public AudioClip dropSound;
    public AudioClip useSound;
    public AudioClip deleteSound;

    public string GetItemName()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return itemNameEN;
        return itemNameES;
    }

    public string GetDescription()
    {
        string lang = Lean.Localization.LeanLocalization.GetFirstCurrentLanguage();
        if (lang == "English") return descriptionEN;
        return descriptionES;
    }
}
