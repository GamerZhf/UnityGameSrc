using System;

public abstract class HashFunction
{
    protected HashFunction()
    {
    }

    public abstract uint GetHash(params int[] data);
    public virtual uint GetHash(int data)
    {
        int[] numArray1 = new int[] { data };
        return this.GetHash(numArray1);
    }

    public virtual uint GetHash(int x, int y)
    {
        int[] data = new int[] { x, y };
        return this.GetHash(data);
    }

    public virtual uint GetHash(int x, int y, int z)
    {
        int[] data = new int[] { x, y, z };
        return this.GetHash(data);
    }

    public int Range(int min, int max, params int[] data)
    {
        return (min + ((int) (((ulong) this.GetHash(data)) % ((long) (max - min)))));
    }

    public int Range(int min, int max, int data)
    {
        return (min + ((int) (((ulong) this.GetHash(data)) % ((long) (max - min)))));
    }

    public float Range(float min, float max, params int[] data)
    {
        return (min + ((this.GetHash(data) * (max - min)) / 4.294967E+09f));
    }

    public float Range(float min, float max, int data)
    {
        return (min + ((this.GetHash(data) * (max - min)) / 4.294967E+09f));
    }

    public int Range(int min, int max, int x, int y)
    {
        return (min + ((int) (((ulong) this.GetHash(x, y)) % ((long) (max - min)))));
    }

    public float Range(float min, float max, int x, int y)
    {
        return (min + ((this.GetHash(x, y) * (max - min)) / 4.294967E+09f));
    }

    public int Range(int min, int max, int x, int y, int z)
    {
        return (min + ((int) (((ulong) this.GetHash(x, y, z)) % ((long) (max - min)))));
    }

    public float Range(float min, float max, int x, int y, int z)
    {
        return (min + ((this.GetHash(x, y, z) * (max - min)) / 4.294967E+09f));
    }

    public float Value(params int[] data)
    {
        return (((float) this.GetHash(data)) / 4.294967E+09f);
    }

    public float Value(int data)
    {
        return (((float) this.GetHash(data)) / 4.294967E+09f);
    }

    public float Value(int x, int y)
    {
        return (((float) this.GetHash(x, y)) / 4.294967E+09f);
    }

    public float Value(int x, int y, int z)
    {
        return (((float) this.GetHash(x, y, z)) / 4.294967E+09f);
    }
}

