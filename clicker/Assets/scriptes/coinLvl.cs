using UnityEngine;

[CreateAssetMenu(fileName = "new coinLvl", menuName = "coinLvl")]
public class coinLvl : ScriptableObject
{
    public int lvl;

    public Sprite sprite;

    public int moneyProfit()
    {
        int total = 1;

        for (int i = 0; i < lvl; i++)
        {
            total *= 10;
        }

        return total;
    }
}
