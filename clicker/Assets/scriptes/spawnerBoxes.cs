using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class spawnerBoxes : MonoBehaviour
{
    [SerializeField] private TMP_Text delayText;
    [SerializeField] private Slider delaySliderSB;//second box

    private Coroutine bc = null; //box create

    private void Awake()
    {
        globalEventManager.onFillSBChanged.AddListener(onFillSBChanged);
    }

    // Start is called before the first frame update
    private void Start()
    {
        bc = StartCoroutine(boxCreate(11 - dinamicDataHolder.purchasedUpgrades[0]));
    }

    private void resetBoxCreate()
    {
        if(bc != null)
            StopCoroutine(bc);
        bc = StartCoroutine(boxCreate(11 - dinamicDataHolder.purchasedUpgrades[0]));
    }

    private void onFillSBChanged()
    {
        delaySliderSB.maxValue = 30 - dinamicDataHolder.purchasedUpgrades[2];

        if(dinamicDataHolder.fillSecondBox >= 30 - dinamicDataHolder.purchasedUpgrades[2])
        {
            dinamicDataHolder.fillSecondBox = 0;

            //create box
        }

        delaySliderSB.value = dinamicDataHolder.fillSecondBox;
    }

    private IEnumerator boxCreate(int delay)
    {
        if (delay == 11)
        {
            yield break;
        }

        delayText.text = "delay: " + delay;

        while (delay > 0)
        {
            yield return new WaitForSeconds(1);

            delay--;

            delayText.text = "delay: " + delay;
        }

        yield return new WaitUntil(() => dinamicDataHolder.allUnitsSmallMap.Count + dinamicDataHolder.countBoxesOnMap < 16);

        GameObject newBox;

        if (staticDataHolder.maps[0].activeInHierarchy)
        {
            newBox = Instantiate(staticDataHolder.standartBoxPref, new Vector3(Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), staticDataHolder.boxSpawnPointY, 0), Quaternion.identity);
        }
        else
        {
            newBox = Instantiate(staticDataHolder.standartBoxPref, new Vector3(Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY), 0), Quaternion.identity);
        }

        newBox.transform.SetParent(staticDataHolder.maps[0].transform);

        newBox.transform.Find("box").GetComponent<box>().wake();

        resetBoxCreate();
    }
}
