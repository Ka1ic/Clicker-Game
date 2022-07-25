using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class moneyText : MonoBehaviour
{
    [SerializeField] private int type = 0;// 0: money; 1: profit; 2: gems

    private Int64 money = dinamicDataHolder.money;

    private float profit = dinamicDataHolder.profit;

    private TMP_Text Text;

    public void Awake()
    {
        Text = GetComponent<TMP_Text>();

        switch (type)
        {
            case 0:
                globalEventManager.onMoneyCountChanged.AddListener(onMoneyCountChaged);
                Text.text = "money: " + functions.convertBigNumber(money);
                break;
            case 1:
                globalEventManager.onProfitNumberChanged.AddListener(onProfitNumberChanged);
                if (profit >= 1e6)
                    Text.text = "coin/sec: " + functions.convertBigNumber(Convert.ToInt64(profit));
                else
                    Text.text = "coin/sec: " + profit;
                break;
        }
    }

    public void onMoneyCountChaged(Int64 value)
    {
        Text.text = "money: " + functions.convertBigNumber(dinamicDataHolder.money);
    }

    public void onProfitNumberChanged(float value)
    {
        profit += value;

        if(profit >= 1e6)
            Text.text = "coin/sec: " + functions.convertBigNumber(Convert.ToInt64(profit));
        else
            Text.text = "coin/sec: " + profit;
    }
}
