using System;
using System.Collections.Generic;

public class ProgressionSystem : IDisposable, ISavable
{
    private PlayerBalance _playerBalance;
    private PlotsModel _plotsModel;
    private SavingData _savingData;
    private Dictionary<int, int> plotUnlockRequirements;
    private HashSet<int> achievedPlots;

    private bool disposed = false;

    public void Initialize(PlayerBalance playerBalance, PlotsModel plotsModel, SavingData savingData)
    {
        _playerBalance = playerBalance;
        _plotsModel = plotsModel;
        _savingData = savingData;
        _playerBalance.OnMoneyChangedEvent += CheckPlotsUnlockConditions;
        InitializePlotUnlockRequirements();
        achievedPlots = new HashSet<int>();
        _savingData.AddToSavables(this);
    }

    public void LoadFromSavingData(SavingData savingData)
    {
        achievedPlots = new HashSet<int>(savingData.AchievedPlots);
    }

    public void PopulateSavingData(SavingData savingData)
    {
        savingData.AchievedPlots.Clear();

        foreach (int index in achievedPlots)
        {
            savingData.AchievedPlots.Add(index);
        }
    }

    public void UnsubscribeFromEvents()
    {
        _playerBalance.OnMoneyChangedEvent -= CheckPlotsUnlockConditions;
    }

    private void InitializePlotUnlockRequirements()
    {
        plotUnlockRequirements = new Dictionary<int, int>();
        plotUnlockRequirements.Add(1, 50); 
        plotUnlockRequirements.Add(2, 100);
        plotUnlockRequirements.Add(3, 150);
        plotUnlockRequirements.Add(4, 200);
        plotUnlockRequirements.Add(5, 250);
    }

    private void CheckPlotsUnlockConditions(int currentMoney)
    {
        foreach (var requirement in plotUnlockRequirements)
        {
            int plotIndex = requirement.Key;
            int requiredMoney = requirement.Value;

            if (currentMoney >= requiredMoney && 
                !_plotsModel.Plots[plotIndex].IsBuyButtonAvaliable && 
                !achievedPlots.Contains(plotIndex))
            {
                _plotsModel.Unlock(plotIndex);
                achievedPlots.Add(plotIndex);
                _savingData.SaveALL();
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
            }

            UnsubscribeFromEvents();
            disposed = true;
        }
    }

    ~ProgressionSystem()
    {
        Dispose(false);
    }
}