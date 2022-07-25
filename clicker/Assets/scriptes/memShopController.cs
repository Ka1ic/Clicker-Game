using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class memShopController : MonoBehaviour
{
    [SerializeField] private GameObject[] products;

    [SerializeField] private GameObject exclamationMark;

    [SerializeField] private bool isMemShop = true;

    private RectTransform content;

    private bool isNewProduct = false;

    private int lvlOfProduct = 0;

    private bool[] news;

    public void wake()
    {
        if (products[0] != null)
            content = products[0].transform.parent.gameObject.GetComponent<RectTransform>();

        globalEventManager.onNewMemUnlocked.AddListener(onNewMemUnlocked);

        if (isMemShop)
        {
            int lastUnloked = dinamicDataHolder.indexLastUnlockedMem;

            for (int i = 0; i < products.Length; i++)
            {
                if (i <= lastUnloked - 4 || i == 0)
                {
                    products[i].SetActive(true);
                }
                else
                    products[i].SetActive(false);
            }

            //size of content
            if (lastUnloked > 4)
            {
                content.sizeDelta = new Vector2(content.sizeDelta.x, 140 * (lastUnloked - 3));
            }
            else
            {
                content.sizeDelta = new Vector2(content.sizeDelta.x, 140);
            }
        }
        else
        {
            unlockNewProduct(0, true);
        }

        checkNews(0, true);
    }

    public void unlockNewProduct(int indexUnlocked = 0, bool firstTime = false)
    {
        lvlOfProduct = indexUnlocked;

        isNewProduct = true;
        if (!this.gameObject.activeInHierarchy || firstTime)
            OnDisable();
    }

    private void onNewMemUnlocked(int lvl)
    {
        if (!isMemShop)
        {
            if (lvl == 2)
                unlockNewProduct(1);
            if(lvl == 5)
                unlockNewProduct(2);
            if (lvl == 6)
                unlockNewProduct(3);
            if (lvl == 7)
                unlockNewProduct(4);
            if (lvl == 10)
                unlockNewProduct(6);
            if (lvl == 12)
                unlockNewProduct(7);
            if (lvl == 17)
                unlockNewProduct(8);
            if (lvl == 18)
                unlockNewProduct(9);
            return;
        }

        if(lvl < 5)
            return;

        lvlOfProduct = lvl;

        isNewProduct = true;
        if (!this.gameObject.activeInHierarchy)
            OnDisable();
    }

    private void OnDisable()
    {
        if(isNewProduct)
        {
            exclamationMark.SetActive(true);

            if (isMemShop)
            {
                content.sizeDelta = new Vector2(content.sizeDelta.x, 140 * (lvlOfProduct - 3));

                for (int i = 0; i < products.Length; i++)
                {
                    products[i].transform.SetParent(content.gameObject.transform);

                    if (i <= lvlOfProduct - 4)
                        products[i].SetActive(true);
                    else
                        products[i].SetActive(false);
                }
            }
            else
            {
                dinamicDataHolder.UnlockedUpgrades[lvlOfProduct] = true;
                int numUnlocked = 0;
                for (int i = 0; i < products.Length; i++)
                {
                    products[i].SetActive(dinamicDataHolder.UnlockedUpgrades[i]);
                    if (dinamicDataHolder.UnlockedUpgrades[i]) numUnlocked++;
                }

                //size of content
                content.sizeDelta = new Vector2(content.sizeDelta.x, 140 * numUnlocked);
            }

            isNewProduct = false;
        }
    }

    public void checkNews(int index, bool isFirstTime = false)
    {
        if(!isFirstTime) 
            news[index] = true;
        else
        {
            news = new bool[products.Length];
            for (int i = 0; i < products.Length; i++)
            {
                if(isMemShop)
                    news[i] = dinamicDataHolder.purchasedUnits[i] > 0 ? true : false;
                else
                    news[i] = dinamicDataHolder.purchasedUpgrades[i] > 0 ? true : false;
            }
        }

        bool isNewsLeft = false;
        if(isMemShop)
        {
            if (!news[0]) isNewsLeft = true;
            for (int i = 1; i <= dinamicDataHolder.indexLastUnlockedMem - 4; i++)
            {
                if (!news[i]) isNewsLeft = true;
            }
        }
        else
        {
            int numUnlocked = 0;
            for (int i = 0; i < products.Length; i++)
                if (dinamicDataHolder.UnlockedUpgrades[i]) numUnlocked++;

            for (int i = 0; i < numUnlocked; i++)
                if (!news[i]) isNewsLeft = true;
        }

        if(!isNewsLeft) 
            exclamationMark.SetActive(false);
        else 
            exclamationMark.SetActive(true);
    }

   /* private void OnEnable()
    {
        exclamationMark.SetActive(false);
    }*/
}
