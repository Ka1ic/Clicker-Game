using System;
using System.Collections.Generic;
using UnityEngine;

public static class dinamicDataHolder
{
    public static bool isCreateTextMoney = true;

    public static bool isCreateParticles = true;

    public static Int64 money = Convert.ToInt64(0);  //не больше 9e18

    public static int numGems = 0; // не больше 2e9

    public static int fillSecondBox = 0;

    public static float profit = 0;

    public static int countBoxesOnMap = 0;

    public static int indexLastUnlockedMem = 0;

    public static bool[] UnlockedUpgrades = new bool[10] { false, false, false, false, false, false, false, false, false, false };

    public static int[] purchasedUpgrades = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static int[] purchasedUnits = new int[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    public static List<GameObject> allUnitsSmallMap = new List<GameObject>(16);

    public static List<GameObject> allUnitsMiddleMap = new List<GameObject>(16);

    public static List<GameObject> allUnitsBigMap = new List<GameObject>(16);

    /////////////////////////////Upgrades///////////////////////////////////////////

/*    public static int lvlOfDelaySpawnBox = 0;
    public static int lvlOfCollector = 0;
    public static int lvlOfSecondTypeBox = 0;
    public static int lvlOfMagnitSmallMap = 0;
    public static int lvlOfBonusesDelaySpawn = 0;
    public static int lvlOfBonusTypes = 0;
    public static int lvlOfBoxDrop = 0;
    public static int lvlOfMagnitMiddleMap = 0;
    public static int lvlOfBonusesDuration = 0;
    public static int lvlOfMagnitBigMap = 0;*/

    public static void changeIsCreateTextMoney(bool value)
    {
        isCreateTextMoney = value;
    }
}
