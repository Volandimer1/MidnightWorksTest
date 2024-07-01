using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private string _itemID;
    [SerializeField] private Sprite _icon;
    [SerializeField] private int _maxStackSize;
    private System.Action _useItemAction;

    public string ItemName
    { get { return _itemName; } }

    public string ItemID
    { get { return _itemID; } }

    public Sprite Icon
    { get { return _icon; } }

    public int MaxStackSize
    { get { return _maxStackSize; } }

    public  System.Action UseItemAction
    { get { return _useItemAction; } }

    public void SetItemUseAction(System.Action useItemAction)
    {
        _useItemAction = useItemAction;
    }
}