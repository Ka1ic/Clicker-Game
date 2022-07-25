using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class secondBoxTypeController : MonoBehaviour
{
    [SerializeField] private Image img;

    [SerializeField] private GameObject btn;

    [SerializeField] private memLvl[] memesSprites;

/*    int[] staticPercents = new int[]
    {

    };*/

    private void OnEnable()
    {
        btn.SetActive(false);

        StartCoroutine(getMem());
    }

    private IEnumerator getMem()
    {
        int collection = dinamicDataHolder.indexLastUnlockedMem + 1;

        List<int> exceptions = new List<int>();

        for (int i = 0; i < 8; i++)
        {
            int randItem = functions.random(0, collection, exceptions: exceptions.ToArray());

            img.sprite = memesSprites[randItem].sprite;

            yield return new WaitForSeconds(.1f);

            if (exceptions.Count >= 4)
                exceptions.RemoveAt(0);

            exceptions.Add(randItem);
        }

        yield return new WaitForSeconds(1);

        /*int[] percents = new int[collection];

        for(int i = 0; i <= collection / 2; i++)
        {
            if(i == 0)
            {
                if(collection % 2 != 0)
                    percents[collection / 2 + 1] = 100 / collection;
            }

            percents[i] = 100 / collection + ((100 / collection) / collection * (collection - i + 1));
            percents[collection - i] = 100 / collection - ((100 / collection) / collection * (collection - i + 1));
        }

        for (int i = 0; i < percents.Length; i++)
        {
            Debug.Log(percents[i]);
        }*/
    }
}
