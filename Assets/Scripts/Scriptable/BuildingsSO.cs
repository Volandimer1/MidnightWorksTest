using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingsSO : ScriptableObject, ISavable
{
    [SerializeField] private List<BuildingAssets> _buildingsList;

    public delegate void BuildingUnlockedDelegate(int buildingIndex);
    public event BuildingUnlockedDelegate OnBuildingUnlockedEvent;

    private SavingData _savingData;

    public List<BuildingAssets> BuildingsList
    { get { return _buildingsList; } }

    public void Initialize(SavingData savingData)
    {
        _savingData = savingData;
        _savingData.AddToSavables(this);
    }

    public void UnlockBuilding(int index)
    {
        if (index >= _buildingsList.Count || index < 0)
        {
            return;
        }

        if (_buildingsList[index].IsAvaliable == false)
        {
            _buildingsList[index].IsAvaliable = true;
            OnBuildingUnlockedEvent?.Invoke(index);
            _savingData.SaveALL();
        }
    }

    public void LoadFromSavingData(SavingData savingData)
    {
        if (savingData.UnlockedBuildingsToBuy.Count == 0)
        {
            _buildingsList[0].IsAvaliable = true;
            return;
        }

        for (int i = 0; i < savingData.UnlockedBuildingsToBuy.Count; i++)
        {
            _buildingsList[i].IsAvaliable = savingData.UnlockedBuildingsToBuy[i];
        }
    }

    public void PopulateSavingData(SavingData savingData)
    {
        savingData.UnlockedBuildingsToBuy.Clear();
        for (int i = 0; i < _buildingsList.Count; i++)
        {
            savingData.UnlockedBuildingsToBuy.Add(_buildingsList[i].IsAvaliable);
        }
    }
}

[System.Serializable] 
public class BuildingAssets
{
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private string _buildingBuyButtonText;
    [SerializeField] private int _moneyIncome;
    private bool _isAvaliable = false;

    public GameObject BuildingPrefab
    { get { return _buildingPrefab; } }

    public string BuildingsBuyButtonText
    { get { return _buildingBuyButtonText; } }

    public int MoneyIncome
    { get { return _moneyIncome; } }

    public bool IsAvaliable
    {
        get { return _isAvaliable; } 
        set { _isAvaliable = value; }
    }
}