using System;
using UnityEngine;

public static class functions
{
    public static string convertBigNumber(Int64 value)
    {
        Int64 num = Convert.ToInt64(value);

        string result = "";

        // 21 - Se, 24 - Sept, 27 - Oct, 30 - Non, 33 - Dec 

        if (num >= 1e18)
            result = num / Convert.ToInt64(1e18) + "." + num % Convert.ToInt64(1e18) / Convert.ToInt64(1e17) + "Qui";
        else if (num >= 1e15)
            result = num / Convert.ToInt64(1e15) + "." + num % Convert.ToInt64(1e15) / Convert.ToInt64(1e14) + "Qua";
        else if(num >= 1e12)
            result = num / Convert.ToInt64(1e12) + "." + num % Convert.ToInt64(1e12) / Convert.ToInt64(1e11) + "Tri";
        else if (num >= 1e9)
            result = num / Convert.ToInt64(1e9) + "." + num % Convert.ToInt64(1e9) / Convert.ToInt64(1e8) + "Bi";
        else if (num >= 1e6)
            result = num / Convert.ToInt64(1e6) + "." + num % Convert.ToInt64(1e6) / Convert.ToInt64(1e5) + "Mi";
        else
            result = value + "";

        return result;
    }

    public static int random(int min, int max, int exception = 0, int[] exceptions = null)
    {
        int[][] intervals;

        if (exceptions == null)
        {
            intervals = new int[2][];

            intervals[0] = new int[] { min, exception };
            intervals[1] = new int[] { exception + 1, max };

            if(min == exception)
            {
                if(exception == max || exception + 1 == max)
                {
                    return min;
                }
                return UnityEngine.Random.Range(intervals[1][0], intervals[1][1]);
            }
            if (exception == max || exception + 1 == max)
            {
                return UnityEngine.Random.Range(intervals[0][0], intervals[0][1]);
            }
        }
        else if(exceptions.Length != 0)
        {
            int countOrdinalExceptions = 0;
            int lastException = min;

            for (int i = 0; i < exceptions.Length + 1; i++)
            {
                if (i == exceptions.Length)
                {
                    if (lastException + 1 == max || lastException == max) countOrdinalExceptions++;

                    continue;
                }

                if (lastException + 1 == exceptions[i] || lastException == exceptions[i]) countOrdinalExceptions++;

                lastException = exceptions[i];
            }
            for (int i = 0; i < exceptions.Length; i++)
            {
                if (i == 0) Debug.Log("exceptions:");
                Debug.Log(exceptions[i]);
            }
            
            intervals = new int[exceptions.Length + 1 - countOrdinalExceptions][];
            Debug.Log("intervalsvcount = " + intervals.Length);
            int index = 0;
            for(int i = 0; i < exceptions.Length + 1; i++)
            {
                if(i == 0)
                {
                    if (min != exceptions[i])
                        intervals[index++] = new int[] { min, exceptions[i] };

                    continue;
                }

                if(i == exceptions.Length)
                {
                    //Debug.Log(intervals.Length + " = " + index);
                    if(exceptions[i - 1] + 1 != max && exceptions[i - 1] != max)
                        intervals[index++] = new int[] { exceptions[i - 1] + 1, max };

                    continue;
                }

                if(exceptions[i - 1] != exceptions[i] && exceptions[i - 1] + 1 != exceptions[i])
                    intervals[index++] = new int[] { exceptions[i - 1] + 1, exceptions[i] };
            }
        }
        else
        {
            intervals = intervals = new int[1][] {new int[] {min, max}};
        }

        int randomInterval = UnityEngine.Random.Range(0, intervals.Length);
        return UnityEngine.Random.Range(intervals[randomInterval][0], intervals[randomInterval][1]);
    }

    public static int randomWithPercentage(int[] percents)
    {
        for (int i = 0; i < percents.Length; i++)
        {
            if(UnityEngine.Random.Range(1, 101) <= percents[i])
            {
                return i;
            }
        }

        return 0;
    }
}
