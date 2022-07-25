using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class globalEventManager
{
    public static UnityEvent<Int64> onMoneyCountChanged = new UnityEvent<Int64>();

    public static UnityEvent<int> onGemsCountChanged = new UnityEvent<int>();

    public static UnityEvent onFillSBChanged = new UnityEvent();// fill second box type

    public static UnityEvent<float> onProfitNumberChanged = new UnityEvent<float>();

    public static UnityEvent<int> onNewMemUnlocked = new UnityEvent<int>();

    public static void sendMoneyCountChanged(Int64 value, bool isPlus = true)
    {
        if(isPlus)
            dinamicDataHolder.money += value;
        else
            dinamicDataHolder.money = value;

        onMoneyCountChanged.Invoke(value);
    }

    public static void sendGemsCountChanged(int value)
    {
        dinamicDataHolder.numGems += value;

        onGemsCountChanged.Invoke(value);
    }

    public static void sendFillSBChanged(int value)
    {
        dinamicDataHolder.fillSecondBox += value;

        onFillSBChanged.Invoke();
    }

    public static void sendProfitNumberChanged(float value)
    {
        dinamicDataHolder.profit += value;

        onProfitNumberChanged.Invoke(value);
    }
    
    public static void sendCountBoxesChanged(int value)
    {
        dinamicDataHolder.countBoxesOnMap += value;
    }

    public static void sendCountUnitsChanged(GameObject unit, bool isAdd)
    {
        if(isAdd)
        {
            if(unit.GetComponent<mem>().lvl.lvl < 6)
            {
                dinamicDataHolder.allUnitsSmallMap.Add(unit);

                unit.transform.SetParent(staticDataHolder.maps[0].transform);
            }
            else if (unit.GetComponent<mem>().lvl.lvl < 12)
            {
                dinamicDataHolder.allUnitsMiddleMap.Add(unit);

                unit.transform.SetParent(staticDataHolder.maps[1].transform);
            }
            else
            {
                dinamicDataHolder.allUnitsBigMap.Add(unit);

                unit.transform.SetParent(staticDataHolder.maps[2].transform);
            }

            sendProfitNumberChanged(unit.GetComponent<mem>().lvl.moneyPerSec());

            checkNewMem(unit.GetComponent<mem>().lvl.lvl);
        }
        else
        {
            if (unit.GetComponent<mem>().lvl.lvl < 6)
            {
                dinamicDataHolder.allUnitsSmallMap.Remove(unit);
            }
            else if (unit.GetComponent<mem>().lvl.lvl < 12)
            {
                dinamicDataHolder.allUnitsMiddleMap.Remove(unit);
            }
            else
            {
                dinamicDataHolder.allUnitsBigMap.Remove(unit);
            }

            sendProfitNumberChanged(-unit.GetComponent<mem>().lvl.moneyPerSec());
        }

        unit.GetComponent<mem>().wake();
    }

    public static void itemsSum(GameObject[] items)
    {
        List<Transform> tr = new List<Transform>();

        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<mem>().StopAllCoroutines();

            tr.Add(items[i].transform);
        }

        Bounds bounds = new Bounds();

        for (int i = 0; i < tr.Count; i++)
        {
            if (i == 0)
            {
                bounds = new Bounds(tr[i].position, Vector3.zero);
            }

            bounds.Encapsulate(tr[i].position);
        }

        for (int i = 0; i < items.Length; i++)
        {
            if(i == 0)
            {
                items[i].GetComponent<mem>().StartCoroutine(items[i].GetComponent<mem>().sum(bounds.center, true));
            }
            else
            {
                items[i].GetComponent<mem>().StartCoroutine(items[i].GetComponent<mem>().sum(bounds.center, false));
            }
        }
    }

    private static void checkNewMem(int lvl)
    {
        if(dinamicDataHolder.indexLastUnlockedMem < lvl)
        {
            dinamicDataHolder.indexLastUnlockedMem++;

            onNewMemUnlocked.Invoke(lvl);
        }
    }
}
