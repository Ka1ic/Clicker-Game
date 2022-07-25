using System;
using System.Collections;
using UnityEngine;

public class textOnMoneyIncrease : MonoBehaviour
{
    private GameObject text;

    public void wake(float profit, bool isMoney = true)
    {
        text = transform.Find("text").gameObject;

        if(isMoney)
        {
            if(profit >= 1e6)
                text.GetComponent<TextMesh>().text = "+" + functions.convertBigNumber(Convert.ToInt64(profit));
            else
                text.GetComponent<TextMesh>().text = "+" + profit;

            StartCoroutine(zoom());

            StartCoroutine(takeOff());
        }
        else
        {
            if (profit >= 1e6)
                text.GetComponent<TextMesh>().text = "+" + functions.convertBigNumber(Convert.ToInt64(profit)) + " c/s";
            else
                text.GetComponent<TextMesh>().text = "+" + profit + " c/s";

            StartCoroutine(zoom(1.5f));

            StartCoroutine(takeOff(35));
        }
    }

    private IEnumerator zoom(float size = 1)
    {
        for (float i = .1f; i < size; i += .9f / 5f)
        {
            text.transform.localScale = new Vector3(i, i);

            yield return new WaitForSeconds(.025f);
        }

        //text.transform.localScale = new Vector3(size, size);

        //StartCoroutine(takeOff());
    }

    private IEnumerator takeOff(int speed = 60)
    {
        for(int i = 0; i < speed; i++)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1f / speed);

            yield return new WaitForSeconds(.01f);
        }

        if(speed != 60)
            yield return new WaitForSeconds(.7f);

        Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }
}
