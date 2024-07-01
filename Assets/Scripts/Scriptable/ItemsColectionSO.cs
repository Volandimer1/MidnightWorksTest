using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemsColectionSO : ScriptableObject
{
    [SerializeField] private List<Item> _itemsList;

    private BuildingsSO _buildingsSO;
    private Dictionary<string, Item> _itemsDictionary = new Dictionary<string, Item>();
    private Dictionary<string, Action> _itemActions = new Dictionary<string, Action>();

    public Dictionary<string, Item> ItemsDictionary
    { get { return _itemsDictionary; } }

    public Dictionary<string, Action> ItemActions
    { get { return _itemActions; } }

    public void Initialize(BuildingsSO buildingsSO)
    {
        _buildingsSO = buildingsSO;
    }

    private void OnEnable()
    {
        HashSet<string> seenItems = new HashSet<string>();
        for (int i = 0; i < _itemsList.Count; i++)
        {
            Item item = _itemsList[i];
            if (item.ItemID != null && seenItems.Contains(item.ItemID))
            {
                Debug.LogWarning($"Duplicate item '{item.ItemID}' found in _itemsList at index {i}. Removing duplicate.");
                _itemsList.RemoveAt(i);
                i--;
            }
            else if (item.ItemID != null)
            {
                seenItems.Add(item.ItemID);
                _itemsDictionary.Add(item.ItemID, item);
                if (item.ItemID == "BuisnessBuilding2")
                {
                    item.SetItemUseAction(UseBuilding2IdeaItem);
                }
            }
        }
    }

    private void UseBuilding2IdeaItem()
    {
        _buildingsSO.UnlockBuilding(1);
    }
}
