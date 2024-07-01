using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SavingData : IUpdatable
{
    public List<PlotSavingInfo> Plots = new List<PlotSavingInfo>();
    public int MoneyAmount;
    public List<bool> UnlockedBuildingsToBuy = new List<bool>();
    public List<int> AchievedPlots = new List<int>();
    public List<InventorySlot> InventoryData = new List<InventorySlot>();

    private const string _savedDataFileName = "SavedData.json";
    private const float _autoSaveInterval = 5f;
    private List<ISavable> _savables = new List<ISavable>();
    private float _timeSpend = 0;

    public void AddToSavables(ISavable savable)
    {
        _savables.Add(savable);
    }

    public void RemoveFromSavables(ISavable savable)
    { 
        _savables.Remove(savable);
    }

    public void SaveALL()
    {
        foreach(ISavable savable in _savables)
        {
            savable.PopulateSavingData(this);
        }

        SaveData();
    }

    public void LoadAll()
    {
        LoadData();

        foreach (ISavable savable in _savables)
        {
            Debug.Log($"{savable.GetType()}");
            savable.LoadFromSavingData(this);
        }
    }

    public void LoadData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, _savedDataFileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(this);
        string filePath = Path.Combine(Application.persistentDataPath, _savedDataFileName);
        File.WriteAllText(filePath, json);
    }

    public void Tick()
    {
        _timeSpend += Time.deltaTime;
        if (_timeSpend >= _autoSaveInterval)
        {
            _timeSpend = 0f;
            SaveALL();
        }
    }
}

[System.Serializable]
public class PlotSavingInfo
{
    public bool IsBuyButtonAvaliable;
    public int BuildingOnPlot;
}