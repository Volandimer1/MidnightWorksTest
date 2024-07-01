using System;
using System.Collections.Generic;
using UnityEngine;

public class PlotsModel : ISavable
{
    public List<PlotInfo> Plots = new List<PlotInfo>();
    private List<PlotBuyButton> _plotsBuyButtons = new List<PlotBuyButton>();
    private SavingData _savingData;

    public void Initialize(List<Transform> plotsPositions, SavingData savingData)
    {
        _savingData = savingData;
        for (int i = 0; i < plotsPositions.Count; i++)
        {
            PlotInfo info = new PlotInfo();
            info.SpawnPosition = plotsPositions[i].position;
            Plots.Add(info);
        }
        _savingData.AddToSavables(this);
    }

    public void Unlock(int plotIndex)
    {
        Plots[plotIndex].IsBuyButtonAvaliable = true;
        _plotsBuyButtons[plotIndex].MakeAvaliable();
    }

    public void AddPlotBuyButton(PlotBuyButton plotBuyButton)
    {
        _plotsBuyButtons.Add(plotBuyButton);
    }

    public void UpdatePlotInfo(int index, bool isBuyButtonAvaliable, int buildingOnPlot, GameObject buildingGameObject)
    {
        Plots[index].IsBuyButtonAvaliable = isBuyButtonAvaliable;
        Plots[index].BuildingOnPlot = buildingOnPlot;
        Plots[index].BuildingGameObject = buildingGameObject;
        _savingData.SaveALL();
    }

    public void LoadFromSavingData(SavingData savingData)
    {
        if (savingData.Plots.Count == 0)
        {
            Plots[0].IsBuyButtonAvaliable = true;
            return;
        }

        for (int i =0; i < savingData.Plots.Count; i++)
        {
            Plots[i].IsBuyButtonAvaliable = savingData.Plots[i].IsBuyButtonAvaliable;
            Plots[i].BuildingOnPlot = savingData.Plots[i].BuildingOnPlot;
        }
    }

    public void PopulateSavingData(SavingData savingData)
    {
        savingData.Plots.Clear();
        for (int i = 0; i < Plots.Count; i++)
        {
            PlotSavingInfo info = new PlotSavingInfo();
            info.IsBuyButtonAvaliable = Plots[i].IsBuyButtonAvaliable;
            info.BuildingOnPlot = Plots[i].BuildingOnPlot;
            savingData.Plots.Add(info);
        }
    }
}

public class PlotInfo
{
    public bool IsBuyButtonAvaliable = false;
    public int BuildingOnPlot = -1;
    public Vector3 SpawnPosition;
    public GameObject BuildingGameObject;
}