using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class Inventory : ISavable
{
    private ItemsColectionSO _levelItems;
    private SavingData _savingData;
    private List<InventorySlot> _slots;
    private Dictionary<string, int> _itemsQuantity = new Dictionary<string, int>();

    public List<InventorySlot> Slots
    { get { return _slots; } }

    public event Action<int, int> OnQuantityChanged;
    public event Action<string, int> OnSlotAdded;
    public event Action<int> OnSlotRemoved;

    public Inventory(ItemsColectionSO levelItems, SavingData savingData)
    {
        _levelItems = levelItems;
        _savingData = savingData;
        _slots = new List<InventorySlot>();
        _savingData.AddToSavables(this);
    }

    public void LoadFromSavingData(SavingData savingData)
    {
        _slots = new List<InventorySlot>(savingData.InventoryData);
        _itemsQuantity.Clear();
        foreach (InventorySlot inventorySlot in _slots)
        {
            AddToTotalQuantity(inventorySlot.ItemID, inventorySlot.Quantity);
        }

    }

    public void PopulateSavingData(SavingData savingData)
    {
        savingData.InventoryData = new List<InventorySlot>(_slots);
    }

    public void AddItem(string itemID, int quantity)
    {
        AddToTotalQuantity(itemID, quantity);
        int remainingQuantity = quantity;

        // Try to add to existing slots
        for (int i = 0; i < _slots.Count; i++)
        {
            InventorySlot slot = _slots[i];
            if (slot.ItemID == itemID && slot.Quantity < _levelItems.ItemsDictionary[itemID].MaxStackSize)
            {
                int availableSpace = _levelItems.ItemsDictionary[itemID].MaxStackSize - slot.Quantity;
                int addQuantity = Math.Min(remainingQuantity, availableSpace);
                slot.Quantity += addQuantity;
                OnQuantityChanged(i, slot.Quantity);
                remainingQuantity -= addQuantity;

                if (remainingQuantity <= 0)
                {
                    _savingData.SaveALL();
                    return;
                }
            }
        }        

        // Add to new slots if needed
        while (remainingQuantity > 0)
        {
            int addQuantity = Math.Min(remainingQuantity, _levelItems.ItemsDictionary[itemID].MaxStackSize);
            _slots.Add(new InventorySlot(itemID, addQuantity));
            OnSlotAdded(itemID, addQuantity);
            remainingQuantity -= addQuantity;
        }
        _savingData.SaveALL();
    }

    public int GetItemQuantity(string itemID)
    {
        int totalQuantity = 0;

        if (_itemsQuantity.ContainsKey(itemID))
        {
            totalQuantity = _itemsQuantity[itemID];
        }

        return totalQuantity;
    }

    public bool RemoveItem(string itemID, int quantity)
    {
        if (GetItemQuantity(itemID) < quantity)
        { 
            return false;
        }

        int remainingQuantity = quantity;

        for (int i = 0; i < _slots.Count; i++)
        {
            InventorySlot slot = _slots[i];
            if (slot.ItemID == itemID)
            {
                if (slot.Quantity >= remainingQuantity)
                {
                    slot.Quantity -= remainingQuantity;
                    if (slot.Quantity == 0)
                    {
                        _slots.RemoveAt(i);
                        OnSlotRemoved(i);
                        i--;
                    }
                    else
                    {
                        OnQuantityChanged(i, slot.Quantity);
                    }
                    break;
                }
                else
                {
                    remainingQuantity -= slot.Quantity;
                    _slots.RemoveAt(i);
                    OnSlotRemoved(i);
                    i--;
                }
            }
        }
        _itemsQuantity[itemID] -= quantity;
        _savingData.SaveALL();
        return true;
    }

    public bool RemoveItem(int slotIndex, int quantity)
    {
        if (slotIndex >= _slots.Count || slotIndex < 0)
        {
            return false;
        }

        InventorySlot slot = _slots[slotIndex];
        if (quantity > slot.Quantity)
        {
            return false;
        }

        slot.Quantity -= quantity;
        if (slot.Quantity == 0)
        {
            _slots.RemoveAt(slotIndex);
            OnSlotRemoved(slotIndex);
        }
        else
        {
            OnQuantityChanged(slotIndex, slot.Quantity);
        }

        _itemsQuantity[slot.ItemID] -= quantity;
        _savingData.SaveALL();
        return true;
    }

    public bool RemoveItem(InventorySlot slot, int quantity)
    {
        if (quantity > slot.Quantity)
        {
            return false;
        }

        slot.Quantity -= quantity;
        int indexOfSlot = _slots.IndexOf(slot);
        if (slot.Quantity == 0)
        {
            _slots.RemoveAt(indexOfSlot);
            OnSlotRemoved(indexOfSlot);
        }
        else
        {
            OnQuantityChanged(indexOfSlot, slot.Quantity);
        }

        _itemsQuantity[slot.ItemID] -= quantity;
        _savingData.SaveALL();
        return true;
    }

    private void AddToTotalQuantity(string itemID, int quantity)
    {
        if (_itemsQuantity.ContainsKey(itemID))
        {
            _itemsQuantity[itemID] += quantity;
        }
        else
        {
            _itemsQuantity.Add(itemID, quantity);
        }
    }
}

[Serializable]
public class InventorySlot
{
    public String ItemID;
    public int Quantity;

    public InventorySlot(string itemID, int quantity)
    {
        ItemID = itemID;
        Quantity = quantity;
    }
}