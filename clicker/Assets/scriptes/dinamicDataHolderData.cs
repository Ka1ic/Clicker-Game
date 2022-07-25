using System;
using UnityEngine;

[System.Serializable]
public class dinamicDataHolderData
{
    public int[] money = new int[1] { 0 };

    public bool isCreateParticles = true;

    public bool isCreateTextMoney = true;

    public int numGems = 0;

    public int fillSecondBox = 0;

    public int countBoxesOnMap = 0;

    public int indexLastUnlockedMem = 0;

    public bool[] UnlockedUpgrades = new bool[10] { false, false, false, false, false, false, false, false, false, false };

    public int[] purchasedUpgrades = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public int[] purchasedUnits = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public int[] allUnitsSmallMap = new int[0];

    public int[] allUnitsMiddleMap = new int[0]; // lvls all units on maps

    public int[] allUnitsBigMap = new int[0];

    public string lastSessionTime = null;

    /////////////////////////////Upgrades///////////////////////////////////////////
/*
    public int lvlOfDelaySpawnBox = 0;
    public int lvlOfCollector = 0;
    public int lvlOfSecondTypeBox = 0;
    public int lvlOfMagnitSmallMap = 0;
    public int lvlOfBonusesDelaySpawn = 0;
    public int lvlOfBonusTypes = 0;
    public int lvlOfBoxDrop = 0;
    public int lvlOfMagnitMiddleMap = 0;
    public int lvlOfBonusesDuration = 0;
    public int lvlOfMagnitBigMap = 0;*/

    public dinamicDataHolderData(bool clear = false)
    {
        if(!clear)
        {
            isCreateTextMoney = dinamicDataHolder.isCreateTextMoney;

            isCreateParticles = dinamicDataHolder.isCreateParticles;

            money = parseLongToInt(dinamicDataHolder.money);

            numGems = dinamicDataHolder.numGems;

            fillSecondBox = dinamicDataHolder.fillSecondBox;

            countBoxesOnMap = dinamicDataHolder.countBoxesOnMap;

            indexLastUnlockedMem = dinamicDataHolder.indexLastUnlockedMem;

            UnlockedUpgrades = dinamicDataHolder.UnlockedUpgrades;

            purchasedUpgrades = dinamicDataHolder.purchasedUpgrades;

            purchasedUnits = dinamicDataHolder.purchasedUnits;

            allUnitsSmallMap = new int[dinamicDataHolder.allUnitsSmallMap.Count];
            for (int i = 0; i < allUnitsSmallMap.Length; i++)
            {
                allUnitsSmallMap[i] = dinamicDataHolder.allUnitsSmallMap[i].GetComponent<mem>().lvl.lvl;
            }

            allUnitsMiddleMap = new int[dinamicDataHolder.allUnitsMiddleMap.Count];
            for (int i = 0; i < allUnitsMiddleMap.Length; i++)
            {
                allUnitsMiddleMap[i] = dinamicDataHolder.allUnitsMiddleMap[i].GetComponent<mem>().lvl.lvl;
            }

            allUnitsBigMap = new int[dinamicDataHolder.allUnitsBigMap.Count];
            for (int i = 0; i < allUnitsBigMap.Length; i++)
            {
                allUnitsBigMap[i] = dinamicDataHolder.allUnitsBigMap[i].GetComponent<mem>().lvl.lvl;
            }

            lastSessionTime = DateTime.Now.ToString();

            //Debug.Log(lastSessionTime);

            /////////////////////////////Upgrades///////////////////////////////////////////

            /*lvlOfDelaySpawnBox = dinamicDataHolder.lvlOfDelaySpawnBox;
            lvlOfCollector = dinamicDataHolder.lvlOfCollector;
            lvlOfSecondTypeBox = dinamicDataHolder.lvlOfSecondTypeBox;
            lvlOfMagnitSmallMap = dinamicDataHolder.lvlOfMagnitSmallMap;
            lvlOfBonusesDelaySpawn = dinamicDataHolder.lvlOfBonusesDelaySpawn;
            lvlOfBonusTypes = dinamicDataHolder.lvlOfBonusTypes;
            lvlOfBoxDrop = dinamicDataHolder.lvlOfBoxDrop;
            lvlOfMagnitMiddleMap = dinamicDataHolder.lvlOfMagnitMiddleMap;
            lvlOfBonusesDuration = dinamicDataHolder.lvlOfBonusesDuration;
            lvlOfMagnitBigMap = dinamicDataHolder.lvlOfMagnitBigMap;*/
        }
    }

    private int[] parseLongToInt(Int64 num)
    {
        int[] result;

        if (num > (Int64)(4e18))
        {
            result = new int[3];

            result[0] = (int)(num / (Int64)(4e18));

            result[1] = (int)(num % (Int64)(4e18) / (Int64)(2e9));

            result[2] = (int)(num % (Int64)(2e9));
        }
        else if (num > (Int64)(2e9))
        {
            result = new int[2];

            result[0] = (int)(num / (Int64)(2e9));

            result[1] = (int)(num % (Int64)(2e9));
        }
        else
        {
            result = new int[1];

            result[0] = (int)(num);
        }

        return result;
    }
}
