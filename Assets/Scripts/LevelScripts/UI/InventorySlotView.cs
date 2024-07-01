using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotView : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _amountText;


    private Inventory _inventory;
    private Action _useItemAction;
    private InventorySlot _slot;

    public void Initialize(Sprite sprite, int amount, Inventory inventory, InventorySlot slot, Action useItemAction)
    {
        _inventory = inventory;
        _slot = slot;
        _useItemAction = useItemAction;
        _image.sprite = sprite;
        UpdateAmountText(amount);
    }

    public void Initialize(Sprite sprite, int amount)
    {
        _image.sprite = sprite;
        UpdateAmountText(amount);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_useItemAction == null)
        {
            return;
        }

        _useItemAction.Invoke();
        _inventory.RemoveItem(_slot, 1);
    }

    public void UpdateAmountText(int amount)
    {
        _amountText.text = amount.ToString();
    }
}