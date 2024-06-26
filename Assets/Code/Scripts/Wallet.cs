﻿using System;
using Data;
using EventBus;
using Events;
using Zenject;

public class Wallet
{
    private readonly PersistentData _persistentData;
    
    [Inject]
    public Wallet(PersistentData persistentData)
    {
        _persistentData = persistentData;
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        _persistentData.PlayerData.Money += amount;
        
        EventBus<OnMoneyChanged>.Invoke(new OnMoneyChanged(from: _persistentData.PlayerData.Money - amount, to: _persistentData.PlayerData.Money));
    }

    public int GetCurrentMoney() => _persistentData.PlayerData.Money;

    public bool IsEnough(int price) => _persistentData.PlayerData.Money - price >= 0;
    
    public void SpendMoney(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        _persistentData.PlayerData.Money -= amount;
        
        EventBus<OnMoneyChanged>.Invoke(new OnMoneyChanged(from: _persistentData.PlayerData.Money - amount, to: _persistentData.PlayerData.Money));
    }
}