using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _inventorySlotPrefab;

    private Inventory _inventory;
    private ItemsColectionSO _levelItems;
    private List<InventorySlotView> _slotViews;

    public void Initialize(Inventory inventory, ItemsColectionSO levelItems)
    {
        _slotViews = new List<InventorySlotView>();
        _inventory = inventory;
        _levelItems = levelItems;
        foreach (var slot in _inventory.Slots)
        {
            HandleSlotAdded(slot.ItemID, slot.Quantity);
        }

        _inventory.OnQuantityChanged += HandleQuantityChanged;
        _inventory.OnSlotAdded += HandleSlotAdded;
        _inventory.OnSlotRemoved += HandleSlotRemoved;
    }

    private void OnDestroy()
    {
        _inventory.OnQuantityChanged -= HandleQuantityChanged;
        _inventory.OnSlotAdded -= HandleSlotAdded;
        _inventory.OnSlotRemoved -= HandleSlotRemoved;
    }

    private void HandleQuantityChanged(int slotIndex, int newQuantity)
    {
        _slotViews[slotIndex].UpdateAmountText(newQuantity);
    }

    private void HandleSlotRemoved(int slotIndex)
    {
        Destroy(_slotViews[slotIndex].gameObject);
        _slotViews.RemoveAt(slotIndex);
    }

    private void HandleSlotAdded(string itemID, int quantity)
    {
        GameObject slotGO = Instantiate(_inventorySlotPrefab, _content);
        InventorySlotView slotView = slotGO.GetComponent<InventorySlotView>();
        Item itemData = _levelItems.ItemsDictionary[itemID];
        slotView.Initialize(itemData.Icon, quantity, _inventory, _inventory.Slots[_inventory.Slots.Count-1], itemData.UseItemAction);
        _slotViews.Add(slotView);
    }
}