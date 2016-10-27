using System;
using UnityEngine;

public class MinMax
{
    public float Max;
    public float Min;

    public MinMax()
    {
    }

    public MinMax(float min, float max)
    {
        this.Min = min;
        this.Max = max;
    }

    public float getRandom()
    {
        return UnityEngine.Random.Range(this.Min, this.Max);
    }
}

