using System;
using UnityEngine;

public class MinMaxInt
{
    public int Max;
    public int Min;

    public MinMaxInt()
    {
    }

    public MinMaxInt(int min, int max)
    {
        this.Min = min;
        this.Max = max;
    }

    public int getRandom()
    {
        return UnityEngine.Random.Range(this.Min, this.Max + 1);
    }
}

