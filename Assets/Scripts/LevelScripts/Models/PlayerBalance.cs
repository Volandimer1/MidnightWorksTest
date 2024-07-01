using UnityEngine;

public class PlayerBalance : ISavable, IUpdatable
{
    public delegate void MoneyChangedDelegate(int newValue);
    public event MoneyChangedDelegate OnMoneyChangedEvent;

    private SavingData _savingData;
    private int _moneyAmount = 0;
    private int _totalIncome = 0;
    private float _timeSpend = 0;
    private const float _incomeInterval = 1f;

    public int MoneyAmount
    { get { return _moneyAmount; } }

    public int TotalIncome
    { get { return _totalIncome; } }

    public void Initialize(SavingData savingData)
    {
        _savingData = savingData;
        _savingData.AddToSavables(this);
    }

    public void AddMoney(int amount)
    {
        _moneyAmount += amount;
        OnMoneyChangedEvent?.Invoke(_moneyAmount);
        _savingData.SaveALL();
    }

    public void RemoveMoney(int amount)
    {
        if (amount > _moneyAmount)
        {
            return;
        }

        _moneyAmount -= amount;
        OnMoneyChangedEvent?.Invoke(_moneyAmount);
        _savingData.SaveALL();
    }

    public void AddToIncome(int amount)
    {
        _totalIncome += amount;
    }

    public void LoadFromSavingData(SavingData savingData)
    {
        _moneyAmount = savingData.MoneyAmount;
    }

    public void PopulateSavingData(SavingData savingData)
    {
        savingData.MoneyAmount = _moneyAmount;
    }

    public void Tick()
    {
        _timeSpend += Time.deltaTime;
        if(_timeSpend >= _incomeInterval)
        {
            _timeSpend = 0f;
            AddMoney(_totalIncome);
        }
    }
}