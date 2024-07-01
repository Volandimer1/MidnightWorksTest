using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlotBuyButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    private BuildService _buildService;
    private GameObject _chooseBuildingCanvas;
    private int _plotIndex;
    private Vector3 _buildLocation;

    public void Initialize(BuildService buildService, int plotIndex, Vector3 buildLocation, GameObject chooseBuildingCanvas)
    {
        _buildService = buildService;
        _plotIndex = plotIndex;
        _buildLocation = buildLocation;
        _chooseBuildingCanvas = chooseBuildingCanvas;
        _button.onClick.AddListener(DoButtonClickedAction);
    }

    public void MakeAvaliable()
    {
        _button.gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void DoButtonClickedAction()
    {
        _buildService.SetBuildLocation(_buildLocation, _plotIndex, _button);
        _chooseBuildingCanvas.SetActive(true);
    }
}
