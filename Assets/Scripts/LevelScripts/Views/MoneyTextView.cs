using TMPro;
using UnityEngine;

public class MoneyTextView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textField;
    private PlayerBalance _playerBalance;

    public void Initialize(PlayerBalance playerBalance)
    {
        _playerBalance = playerBalance;
        _textField.text = "= " + _playerBalance.MoneyAmount + "$";
        _playerBalance.OnMoneyChangedEvent += ChangeText;
    }

    private void ChangeText(int newValue)
    {
        _textField.text = "= " + newValue + "$";
    }

    private void OnDestroy()
    {
        _playerBalance.OnMoneyChangedEvent -= ChangeText;
    }
}
