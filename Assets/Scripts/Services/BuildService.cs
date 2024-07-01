using UnityEngine;
using UnityEngine.UI;

public class BuildService
{
    private Vector3 _currentBuildSpot;
    private int _plotIndex;
    private PlotsModel _plotsModel;
    private BuildingFactory _buildingFactory;
    private GameObject _buttonOnPlotThatCalledConstruction;

    public void Initialize(PlotsModel plotsModel, BuildingFactory buildingFactory)
    {
        _plotsModel = plotsModel;
        _buildingFactory = buildingFactory;
    }

    public void SetBuildLocation(Vector3 currentBuildSpot, int plotIndex, Button button)
    {
        _currentBuildSpot = currentBuildSpot;
        _plotIndex = plotIndex;
        _buttonOnPlotThatCalledConstruction = button.gameObject;
    }

    public void Construct(int buildingIndex)
    {
        _plotsModel.UpdatePlotInfo(_plotIndex, false, buildingIndex, _buildingFactory.Get(buildingIndex, _currentBuildSpot));
        _buttonOnPlotThatCalledConstruction.gameObject.SetActive(false);
    }
}