using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    public Player player;
    public int currency;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currency)
        {
            Debug.Log("You not have enough money to buy " + _price);
            return false;
        }
        
        currency = currency - _price;
        return true;
    }

    private void Update()
    {
       
    }

    public int CurrentCurrencyAmount()
    {
        return currency;
    }

    public void LoadData(GameData _data)
    {
        this.currency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currency;
    }
}
