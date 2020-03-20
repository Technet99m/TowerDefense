﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney instance;
    [SerializeField] float money;
    public float Money
    { 
        get { return money; }
        set
        { 
            money = value;
            RefreshText();
        }
    }
    public float TowerPrice, ColorPrice, SmartPrice;

    [SerializeField] CounterFitter counter;

    private void Awake()
    {
        instance = this;
        RefreshText();
        WaveManager.EnemyKilled += OnEnemyDied;
    }

    void OnEnemyDied(object sender, EventArgs e)
    {
        AddMoney(1f);
    }
    void RefreshText()
    {
        counter.SetCounterTo(Money, 1);
    }
    public void AddMoney(float change)
    {
        if (change < 0)
            return;
        Money += change;
        RefreshText();
    }
    public bool TryBuyForPrice(float price)
    {
        if (price < 0)
            return false;
        
        if (Money >= price)
        {
            Money -= price;
            RefreshText();
            return true;
        }
        // Activate Not enough money panel
        return false;
    }
    public bool isEnough(float price)
    {
        return Money >= price;
    }
}
