using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class memChangeLocation : MonoBehaviour
{
    public void wake(memLvl lvl)
    {
        if(lvl.sprite != null)
            this.gameObject.GetComponent<SpriteRenderer>().sprite = lvl.sprite;

        this.transform.localScale = new Vector3(lvl.sizeX, lvl.sizeY, 1);

        if (this.gameObject.activeInHierarchy)
            StartCoroutine(zoom(lvl));
        else
            Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator zoom(memLvl lvl)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        for(float i = 1; i < 15; i += .25f)
        {
            this.transform.localScale = new Vector3(lvl.sizeX * i, lvl.sizeY * i, 1);

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - i / 15f);

            yield return new WaitForSeconds(.006f);
        }

        Destroy(this.gameObject);
    }
}
