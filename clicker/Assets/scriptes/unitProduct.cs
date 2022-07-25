using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class unitProduct : MonoBehaviour
{
    [SerializeField] private int lvl = 0;

    [SerializeField] private bool isMem = true;

    [SerializeField] private GameObject exclamationMark;

    private Int64 price = 500;

    private GameObject buttonMoney;
    private Image imgMB; // image money button
    private TMP_Text textMB; // text money button
    private TMP_Text textNP; // text number purchased


    private GameObject windowULE; //window Units Limit Exceeded


    private Slider sliderLvl;
    private TMP_Text effectLvl;
    private TMP_Text textLvl;

    private int[] maxLvls;
    private Dictionary<int, string[]> effectsTexts = new Dictionary<int, string[]>()
    {
        {0, new string[]{"0s -> 10s", "10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "6s -> 5s", "5s -> 4s", "4s -> 3s", "3s -> 2s", "MAX (2s)"} },
        {1, new string[]{"0h -> 2h", "2h -> 2.5h", "2.5h -> 3h", "3h -> 4.5h", "4.5h -> 5h", "5h -> 5.5h", "5.5h -> 6h", "MAX(6h)" } },
        {2, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {3, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {4, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {5, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {6, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {7, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {8, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } },
        {9, new string[]{"10s -> 9s", "9s -> 8s", "8s -> 7s", "7s -> 6s", "MAX (6s)" } }
    };
    private Int64[][] prices = new Int64[10][]
    {
        new Int64[] {200, 3000, (Int64)1e4, (Int64)1e5, (Int64)1e6, (Int64)1e7, (Int64)1e8, (Int64)1e9, (Int64)1e10},
        new Int64[] {1,2,3,4,5,6,7},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4},
        new Int64[] {1,2,3,4}
    };

    private bool isGems = false;
    //private int priceGems = 1;

    private memShopController memSC = null;

    private void Awake()
    {
        maxLvls = new int[effectsTexts.Count];
        for(int i = 0; i < effectsTexts.Count; i++)
        {
            maxLvls[i] = effectsTexts[i].Length - 1;
        }

        buttonMoney = transform.Find("buy button").gameObject;
        textMB = buttonMoney.transform.Find("button text").gameObject.GetComponent<TMP_Text>();
        imgMB = buttonMoney.GetComponent<Image>();

        if (isMem)
        {
            textNP = transform.Find("numer purchased").gameObject.GetComponent<TMP_Text>();
            windowULE = transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.Find("menu limit exceeded").gameObject;
        }
        else
        {
            sliderLvl = transform.Find("Slider lvl").gameObject.GetComponent<Slider>();
            textLvl = sliderLvl.gameObject.transform.Find("lvl text").gameObject.GetComponent<TMP_Text>();
            effectLvl = transform.Find("effect value").gameObject.GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        if (isMem)
        {
            if (staticDataHolder.memesLvls[lvl].sprite != null)
                transform.Find("background").transform.Find("sprite").gameObject.GetComponent<Image>().sprite = staticDataHolder.memesLvls[lvl].sprite;

            if(staticDataHolder.memesLvls[lvl].moneyPerSec() >= 1e6)
                transform.Find("profit").gameObject.GetComponent<TMP_Text>().text = "produces " + functions.convertBigNumber(Convert.ToInt64(staticDataHolder.memesLvls[lvl].moneyPerSec())) + " c/s";
            else
                transform.Find("profit").gameObject.GetComponent<TMP_Text>().text = "produces " + staticDataHolder.memesLvls[lvl].moneyPerSec() + " c/s";

            purchasedUnitsNumberChanged(true);
        }
        else
        {
            purchasedUpgradesNumberChanged(true);
        }
    }

    void Update()
    {
        if (isGems) return;

        if (price > dinamicDataHolder.money)
        {
            imgMB.color = new Color(imgMB.color.r, imgMB.color.g, imgMB.color.b, .5f);
        }
        else
        {
            imgMB.color = new Color(imgMB.color.r, imgMB.color.g, imgMB.color.b, 1);
        }
    }

    public void onClickMoneyButton()
    {
        if(!isMem)
        {
            if(isGems)
            {
                return;
            }

            if(price <= dinamicDataHolder.money)
            {
                globalEventManager.sendMoneyCountChanged(-price);

                purchasedUpgradesNumberChanged();
            }

            return;
        }

        if (dinamicDataHolder.money >= price)
        {
            if (lvl < 6)
            {
                if (dinamicDataHolder.allUnitsSmallMap.Count + dinamicDataHolder.countBoxesOnMap < 16)
                {
                    globalEventManager.sendMoneyCountChanged(-price);

                    purchasedUnitsNumberChanged();
                }
                else
                {
                    windowULE.SetActive(true);
                }
            }
            else if (lvl < 12)
            {
                if (dinamicDataHolder.allUnitsMiddleMap.Count < 16)
                {
                    globalEventManager.sendMoneyCountChanged(-price);

                    purchasedUnitsNumberChanged();
                }
                else
                {
                    windowULE.SetActive(true);
                }
            }
            else
            {
                if (dinamicDataHolder.allUnitsBigMap.Count < 16)
                {
                    globalEventManager.sendMoneyCountChanged(-price);

                    purchasedUnitsNumberChanged();
                }
                else
                {
                    windowULE.SetActive(true);
                }
            }
        }
    }

    //public void onClickGemsButton()
    //{
    //    if (dinamicDataHolder.numGems >= priceGems)
    //    {
    //        if (lvl < 6)
    //        {
    //            if (dinamicDataHolder.allUnitsSmallMap.Count + dinamicDataHolder.countBoxesOnMap < 16)
    //            {
    //                globalEventManager.sendGemsCountChanged(-priceGems);

    //                purchasedUnitsNumberChanged();
    //            }
    //            else
    //            {
    //                windowULE.SetActive(true);
    //            }
    //        }
    //        else if (lvl < 12)
    //        {
    //            if(dinamicDataHolder.allUnitsMiddleMap.Count < 16)
    //            {
    //                globalEventManager.sendGemsCountChanged(-priceGems);

    //                purchasedUnitsNumberChanged();
    //            }
    //            else
    //            {
    //                windowULE.SetActive(true);
    //            }
    //        }
    //        else
    //        {
    //            if (dinamicDataHolder.allUnitsBigMap.Count < 16)
    //            {
    //                globalEventManager.sendGemsCountChanged(-priceGems);

    //                purchasedUnitsNumberChanged();
    //            }
    //            else
    //            {
    //                windowULE.SetActive(true);
    //            }
    //        }
    //    }
    //}

    private void purchasedUnitsNumberChanged(bool firstTime = false)
    {
        if(firstTime)
        {
            if (dinamicDataHolder.purchasedUnits[lvl] > 0) changeNews();

            for (int i = 0; i < lvl; i++)
            {
                price *= 3;
            }

            for (int i = 0; i < dinamicDataHolder.purchasedUnits[lvl]; i++)
            {
                price = Convert.ToInt32(Math.Round(price * .1115f)) * 10;
            }

            textMB.text = functions.convertBigNumber(price);

            textNP.text = "- " + dinamicDataHolder.purchasedUnits[lvl] + " purchased";

            return;
        }

        dinamicDataHolder.purchasedUnits[lvl] += 1;
        changeNews();
        textNP.text = "- " + dinamicDataHolder.purchasedUnits[lvl] + " purchased";
        price = Convert.ToInt32(Math.Round(price * .1115f)) * 10;
        textMB.text = functions.convertBigNumber(price);

        GameObject newUnit = Instantiate(staticDataHolder.unitPref, new Vector3(UnityEngine.Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), UnityEngine.Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY), 0), Quaternion.identity);
        newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[lvl];
        globalEventManager.sendCountUnitsChanged(newUnit, true);
    }

    private void purchasedUpgradesNumberChanged(bool firstTime = false)
    {
        if (!firstTime)
        {
            dinamicDataHolder.purchasedUpgrades[lvl]++;
            changeNews();
        } 
        else if (dinamicDataHolder.purchasedUpgrades[lvl] > 0) changeNews();

        useNewUpgrade();

        if(dinamicDataHolder.purchasedUpgrades[lvl] != maxLvls[lvl])
        {
            textLvl.text = "Level " + dinamicDataHolder.purchasedUpgrades[lvl] + "/" + maxLvls[lvl];
            sliderLvl.value = (float)(dinamicDataHolder.purchasedUpgrades[lvl]) / (float)(maxLvls[lvl]);
            effectLvl.text = effectsTexts[lvl][dinamicDataHolder.purchasedUpgrades[lvl]];

            price = prices[lvl][dinamicDataHolder.purchasedUpgrades[lvl]];
            textMB.text = functions.convertBigNumber(price);
        }
        else
        {
            sliderLvl.gameObject.SetActive(false);
            buttonMoney.SetActive(false);
            effectLvl.text = effectsTexts[lvl][dinamicDataHolder.purchasedUpgrades[lvl]];
        }
    }

    private void useNewUpgrade()
    {
        switch (lvl)
        {
            case 0:
                /*dinamicDataHolder.lvlOfDelaySpawnBox = dinamicDataHolder.purchasedUpgrades[lvl];*/
                if (dinamicDataHolder.purchasedUpgrades[lvl] == 1)
                    GameObject.Find("game manager").GetComponent<spawnerBoxes>().StartCoroutine("boxCreate", 11 - dinamicDataHolder.purchasedUpgrades[lvl]);
                break;
        }
        
    }

    private void changeNews()
    {
        if (memSC == null) memSC = transform.parent.transform.parent.transform.parent.transform.parent.gameObject.GetComponent<memShopController>();

        exclamationMark.SetActive(false);

        memSC.checkNews(lvl);
    }
}
