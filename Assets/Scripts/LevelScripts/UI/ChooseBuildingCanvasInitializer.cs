using UnityEngine;
using UnityEngine.UI;

public class ChooseBuildingCanvasInitializer : MonoBehaviour
{
    [SerializeField] private Transform _contentHolder;
    [SerializeField] private GameObject _buildingsBuyButtonPrefab;

    public void Initialize(BuildingsSO buildingsSO, BuildService buildService, GameObject chooseCanvas, PlayerBalance playerBalance)
    {
        for (int i = 0; i < buildingsSO.BuildingsList.Count; i++)
        {
            GameObject buttonObject = Instantiate(_buildingsBuyButtonPrefab, _contentHolder);
            BuyBuildingButton button = buttonObject.GetComponent<BuyBuildingButton>();
            button.Initialize(buildService, i, buildingsSO, chooseCanvas, playerBalance, buildingsSO.BuildingsList[i].BuildingsBuyButtonText);
            if (buildingsSO.BuildingsList[i].IsAvaliable)
            {
                buttonObject.gameObject.GetComponent<Button>().interactable = true;
            }
            else
            {
                buttonObject.gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
}
