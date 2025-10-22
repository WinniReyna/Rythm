using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private InventorySO inventorySO;
    [SerializeField] private ItemDatabase itemDatabase;

    private void OnEnable()
    {
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = false;
    }

    private void OnDisable()
    {
        if (PlayerMovement.Instance != null)
            PlayerMovement.Instance.canMove = true;
    }

    public void RefreshUI()
    {
        foreach (Transform child in itemsParent)
            Destroy(child.gameObject);

        foreach (var invItem in inventorySO.items)
        {
            ItemSO itemSO = itemDatabase.GetItemByID(invItem.itemID);
            if (itemSO == null) continue;

            GameObject go = Instantiate(itemPrefab, itemsParent);
            InventoryUIItem uiItem = go.GetComponent<InventoryUIItem>();
            uiItem.Setup(itemSO, invItem.quantity, inventorySO, itemDatabase);
        }
    }
}





