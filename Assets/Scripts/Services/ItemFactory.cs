using UnityEngine;

public class ItemFactory
{
    private Camera _camera;
    private GameObject _itemDropPrefab;
    private Inventory _inventory;
    private ItemsColectionSO _itemsColectionSO;

    public ItemFactory(GameObject itemDropPrefab, Inventory inventory, Camera camera, ItemsColectionSO itemsColectionSO)
    {
        _itemDropPrefab = itemDropPrefab;
        _inventory = inventory;
        _camera = camera;
        _itemsColectionSO = itemsColectionSO;
    }

    public ItemDrop GetItemDrop(string itemID, Vector3 position)
    {
        if (_itemsColectionSO.ItemsDictionary.TryGetValue(itemID, out Item item))
        {
            GameObject itemDropInstance = GameObject.Instantiate(_itemDropPrefab, position, Quaternion.identity);
            ItemDrop itemDrop = itemDropInstance.GetComponent<ItemDrop>();
            itemDrop.Initialize(item, _inventory, _camera);
            return itemDrop;
        }
        else
        {
            Debug.LogError($"Item with ID {itemID} not found in the item database.");
            return null;
        }
    }
}