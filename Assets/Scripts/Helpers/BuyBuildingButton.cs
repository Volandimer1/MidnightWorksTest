using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BuyBuildingButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    private BuildService _buildService;
    private BuildingsSO _buildingsSO;
    private GameObject _chooseCanvas;
    private PlayerBalance _playerBalance;
    private int _index;

    public void Initialize(BuildService buildService, int index, BuildingsSO buildingsSO, GameObject chooseCanvas, PlayerBalance playerBalance, string buttonText)
    {
        _buildService = buildService;
        _buildingsSO = buildingsSO;
        _chooseCanvas = chooseCanvas;
        _playerBalance = playerBalance;
        _index = index;
        _buttonText.text = buttonText;
        _button.onClick.AddListener(DoButtonClickedAction);
        _buildingsSO.OnBuildingUnlockedEvent += UnlockButton;
    }

    public void OnDestroy()
    {
        _buildingsSO.OnBuildingUnlockedEvent -= UnlockButton;
    }

    private void DoButtonClickedAction()
    {
        _playerBalance.AddToIncome(_buildingsSO.BuildingsList[_index].MoneyIncome);
        _buildService.Construct(_index);
        _chooseCanvas.SetActive(false);
    }

    private void UnlockButton(int unlockedBuildingIndex)
    {
        if (unlockedBuildingIndex == _index)
        {
            _button.interactable = true;
        }
    }
}
