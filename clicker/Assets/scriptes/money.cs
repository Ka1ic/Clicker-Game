using System.Collections;
using UnityEngine;

public class money : MonoBehaviour
{
    private int profit = 0;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void on(coinLvl moneyLvl)
    {
        profit = moneyLvl.moneyProfit();

        StartCoroutine(spawn());
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    IEnumerator spawn()
    {
        for(float i = .1f; i < .5f; i += .1f)
        {
            transform.localScale = new Vector3(i, i);

            yield return new WaitForSeconds(.01f);
        }

        transform.localScale = new Vector3(.5f, .5f);

        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        yield return new WaitForSeconds(.7f);

        for(float i = 1; i > -.05f; i -= .05f)
        {
            Color color = sr.color;

            color.a = i;

            sr.color = color;

            yield return new WaitForSeconds(.015f);
        }

        if(dinamicDataHolder.isCreateTextMoney)
        {
            GameObject text = Instantiate(staticDataHolder.textPref, transform.position, Quaternion.identity);

            text.transform.SetParent(transform.parent);

            text.GetComponent<textOnMoneyIncrease>().wake(profit);
        }

        Destroy(this.gameObject);
    }

    public void OnDestroy()
    {
        globalEventManager.sendMoneyCountChanged(profit);
    }
}
