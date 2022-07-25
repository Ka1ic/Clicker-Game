using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new memLvl", menuName = "memLvl")]
public class memLvl : ScriptableObject
{
    public int lvl;

    public Sprite sprite;

    public float sizeX = 1f, sizeY = 1f;

    private float[] profitList = { .5f, 1.5f, 4, 9.5f, 21, 44.5f, 92, 187.5f, 397, 762.5f, 1530, 3065.5f, 6137, 12280.5f, 24568, 49143.5f, 98259, 196598.5f };

    public float moneyPerSec()
    {
        return profitList[lvl];
    }

    public int[] getMoneyBehavior()
    {
        int i = 0;
        bool end = false;
        List<int> li = new List<int>();

        while (!end)
        {
            li = new List<int>();

            i++;

            int acum = Convert.ToInt32(profitList[lvl] * 2) * i;

            for (int j = 1000000; j > 0; j /= 10)
            {
                while (acum / j > 0)
                {
                    acum -= j;

                    li.Add(Convert.ToInt16(Mathf.Log10(j)));
                }
            }

            if (li.Count == i && acum == 0)
            {
                end = true;
            }
        }

        li = shuffleList(li);

        return li.ToArray();
    }

    private List<int> shuffleList(List<int> li)
    {
        for (int i = li.Count - 1; i >= 1; i--)
        {
            int j = UnityEngine.Random.Range(0 , i + 1);

            var temp = li[j];
            li[j] = li[i];
            li[i] = temp;
        }

        return li;
    }
}
