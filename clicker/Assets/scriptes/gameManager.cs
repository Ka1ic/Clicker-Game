using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject moneyPref;
    [SerializeField] private GameObject unitPref;
    [SerializeField] private GameObject unitAnimPref;
    [SerializeField] private GameObject standartBoxPref;
    [SerializeField] private GameObject textPref;

    [SerializeField] private coinLvl[] coinsLvls;
    [SerializeField] private memLvl[] memesLvls;

    [SerializeField] private GameObject loadWindow;
    [SerializeField] private GameObject collectorWindow;
    [SerializeField] private TMP_Text collectorText;
    [SerializeField] private Toggle iMTCToggle; //isMoneyTextCreate
    [SerializeField] private Toggle iCPToggle; //isParticlesCreate
    [SerializeField] private GameObject[] maps;
    [SerializeField] private memShopController[] memShopControllers;

    [SerializeField] private bool isClear = false;

    private bool isSmallMapFillUp = false;
    private bool isMiddleMapFillUp = false;
    private bool isBigMapFillUp = false;

    private bool isStartEnded = false;

    public void Awake()
    {
        staticDataHolder.moneyPref = moneyPref;

        staticDataHolder.unitPref = unitPref;

        staticDataHolder.unitAnimPref = unitAnimPref;

        staticDataHolder.coinsLvls = coinsLvls;

        staticDataHolder.memesLvls = memesLvls;

        staticDataHolder.standartBoxPref = standartBoxPref;

        staticDataHolder.maps = maps;

        staticDataHolder.textPref = textPref;
    }

    private void Start()
    {
        load();

        isStartEnded = true;
    }

    private void OnApplicationQuit()
    {
        if(isStartEnded)
            saveSystem.saveDinamicDataHolder();
    }

    private void OnApplicationPause(bool pause)
    {
        if(isStartEnded)
            saveSystem.saveDinamicDataHolder();
    }

    private void load()
    {
        loadWindow.SetActive(true);
        if (isClear)
        {
            saveSystem.clearData();
            globalEventManager.sendMoneyCountChanged(3000);
            loadWindow.SetActive(false);
            for (int i = 0; i < memShopControllers.Length; i++)
                memShopControllers[i].wake();
            return;
        }


        dinamicDataHolderData data = saveSystem.loadDinamicDataHolder();

        if (data != null)
        {
            if (data.money.Length == 3)
                globalEventManager.sendMoneyCountChanged(data.money[0] * (Int64)(4e18) + data.money[1] * (Int64)(2e9) + data.money[2], false);
            else if (data.money.Length == 2)
                globalEventManager.sendMoneyCountChanged(data.money[0] * (Int64)(2e9) + data.money[1], false);
            else if (data.money.Length == 1)
                globalEventManager.sendMoneyCountChanged(data.money[0], false);

            globalEventManager.sendGemsCountChanged(data.numGems);

            globalEventManager.sendFillSBChanged(data.fillSecondBox);

            dinamicDataHolder.indexLastUnlockedMem = data.indexLastUnlockedMem;

            dinamicDataHolder.UnlockedUpgrades = data.UnlockedUpgrades;

            dinamicDataHolder.purchasedUnits = data.purchasedUnits;

            dinamicDataHolder.purchasedUpgrades = data.purchasedUpgrades;

            iMTCToggle.isOn = data.isCreateTextMoney;
            dinamicDataHolder.isCreateTextMoney = data.isCreateTextMoney;

            iCPToggle.isOn = data.isCreateParticles;
            dinamicDataHolder.isCreateParticles = data.isCreateParticles;

            /////////////////////////Upgrades/////////////////////////////
            /*dinamicDataHolder.lvlOfDelaySpawnBox = data.lvlOfDelaySpawnBox;
            dinamicDataHolder.lvlOfCollector = data.lvlOfCollector;
            Debug.Log(data.lvlOfCollector);
            dinamicDataHolder.lvlOfSecondTypeBox = data.lvlOfSecondTypeBox;
            dinamicDataHolder.lvlOfMagnitSmallMap = data.lvlOfMagnitSmallMap;
            dinamicDataHolder.lvlOfBonusesDelaySpawn = data.lvlOfBonusesDelaySpawn;
            dinamicDataHolder.lvlOfBonusTypes = data.lvlOfBonusTypes;
            dinamicDataHolder.lvlOfBoxDrop = data.lvlOfBoxDrop;
            dinamicDataHolder.lvlOfMagnitMiddleMap = data.lvlOfMagnitMiddleMap;
            dinamicDataHolder.lvlOfBonusesDuration = data.lvlOfBonusesDuration;
            dinamicDataHolder.lvlOfMagnitBigMap = data.lvlOfMagnitBigMap;*/
            //////////////////////////////////////////////////////////////

            for (int i = 0; i < data.countBoxesOnMap; i++)
            {
                GameObject newBox = Instantiate(staticDataHolder.standartBoxPref, new Vector2(UnityEngine.Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), UnityEngine.Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY)), Quaternion.identity);

                newBox.transform.Find("box").gameObject.GetComponent<box>().wake();
            }

            isSmallMapFillUp = true;
            isMiddleMapFillUp = true;
            isBigMapFillUp = true;

            StartCoroutine(unitsCreate(data, 0));
            StartCoroutine(unitsCreate(data, 1));
            StartCoroutine(unitsCreate(data, 2));

            if (data.lastSessionTime != null)
            {
                collectorEnable(data.lastSessionTime);
            }

            for (int i = 0; i < memShopControllers.Length; i++)
                memShopControllers[i].wake();
        }
        else
        {
            globalEventManager.sendMoneyCountChanged(3000);

            loadWindow.SetActive(false);
        }
    }

    private void collectorEnable(string lastSessionTime)
    {
        int lvl = dinamicDataHolder.purchasedUpgrades[1];
        if (lvl == 0) return;
        else
        {
            TimeSpan pauseTime = DateTime.Now - DateTime.Parse(lastSessionTime);
            int pauseMinutes = (int)pauseTime.TotalMinutes;
            int collectorWorkTime = (4 + lvl) * 30;
            if (pauseMinutes >= 5)
            {
                collectorWindow.SetActive(true);
                Int64 profit = 0;
                if (pauseMinutes >= collectorWorkTime)
                {
                    profit = (Int64)(collectorWorkTime * dinamicDataHolder.profit * 60);
                }
                else
                {
                    profit = (Int64)(pauseMinutes * dinamicDataHolder.profit * 60);
                }

                globalEventManager.sendMoneyCountChanged(profit);
                collectorText.text = "money gain: " + functions.convertBigNumber(profit) + " coin";
            }
        }
    }

    private IEnumerator unitsCreate(dinamicDataHolderData data, int indexMap = 0)
    {
        if (indexMap == 0)
        {
            //dinamicDataHolder.allUnitsSmallMap.Clear();

            for (int i = 0; i < data.allUnitsSmallMap.Length; i++)
            {
                GameObject newUnit = Instantiate(staticDataHolder.unitPref, new Vector2(UnityEngine.Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), UnityEngine.Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY)), Quaternion.identity);

                newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[data.allUnitsSmallMap[i]];

                globalEventManager.sendCountUnitsChanged(newUnit, true);

                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            }

            isSmallMapFillUp = false;
        }
        else if(indexMap == 1)
        {
            //dinamicDataHolder.allUnitsMiddelMap.Clear();

            for (int i = 0; i < data.allUnitsMiddleMap.Length; i++)
            {
                GameObject newUnit = Instantiate(staticDataHolder.unitPref, new Vector2(UnityEngine.Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), UnityEngine.Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY)), Quaternion.identity);

                newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[data.allUnitsMiddleMap[i]];

                globalEventManager.sendCountUnitsChanged(newUnit, true);

                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            }

            isMiddleMapFillUp = false;
        }
        else if(indexMap == 2)
        {
            //dinamicDataHolder.allUnitsBigMap.Clear();

            for (int i = 0; i < data.allUnitsBigMap.Length; i++)
            {
                GameObject newUnit = Instantiate(staticDataHolder.unitPref, new Vector2(UnityEngine.Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), UnityEngine.Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY)), Quaternion.identity);

                newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[data.allUnitsBigMap[i]];

                globalEventManager.sendCountUnitsChanged(newUnit, true);

                yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));
            }

            isBigMapFillUp = false;
        }

        if (!isBigMapFillUp && !isMiddleMapFillUp && !isSmallMapFillUp)
        {
            loadWindow.SetActive(false);
        }
    }

    public void changeBoolInDinamicDataHolder(int index)
    {
        if(index == 0)
            dinamicDataHolder.isCreateTextMoney = iMTCToggle.isOn;
        else if(index == 1)
            dinamicDataHolder.isCreateParticles = iCPToggle.isOn;
    }
}
