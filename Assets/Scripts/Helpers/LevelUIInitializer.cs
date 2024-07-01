using UnityEngine;
using UnityEngine.UI;

public class LevelUIInitializer : MonoBehaviour
{
    [SerializeField] private MoneyTextView _moneyTextView;
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _optionButton;

    private PopUpService _popUpService;
    private GameStateMachine _gameStateMachine;
    private GameObject _inventoryCanvavs;

    public void Initialize(PlayerBalance playerBalance, PopUpService popUpService, GameStateMachine gameStateMachine, GameObject inventoryCanvavs)
    {
        _popUpService = popUpService;
        _gameStateMachine = gameStateMachine;
        _inventoryCanvavs = inventoryCanvavs;
        _moneyTextView.Initialize(playerBalance);
        _optionButton.onClick.AddListener(DoSettingsButtonClickedAction);
        _inventoryButton.onClick.AddListener(HandleInventoryButtonClick);
    }

    private void HandleInventoryButtonClick()
    {
        _inventoryCanvavs.gameObject.SetActive(!_inventoryCanvavs.gameObject.activeSelf);
    }

    private void DoSettingsButtonClickedAction()
    {
        _popUpService.ShowOptionsPopUp(_gameStateMachine);
    }

    private void OnDestroy()
    {
        _optionButton.onClick.RemoveListener(DoSettingsButtonClickedAction);
        _inventoryButton.onClick.RemoveListener(HandleInventoryButtonClick);
    }
}
