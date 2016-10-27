using System;

public class XXHash : HashFunction
{
    private const uint PRIME32_1 = 0x9e3779b1;
    private const uint PRIME32_2 = 0x85ebca77;
    private const uint PRIME32_3 = 0xc2b2ae3d;
    private const uint PRIME32_4 = 0x27d4eb2f;
    private const uint PRIME32_5 = 0x165667b1;
    public uint seed;

    public XXHash(int seed)
    {
        this.seed = (uint) seed;
    }

    private static uint CalcSubHash(uint value, uint read_value)
    {
        value += read_value * 0x85ebca77;
        value = RotateLeft(value, 13);
        value *= 0x9e3779b1;
        return value;
    }

    private static uint CalcSubHash(uint value, byte[] buf, int index)
    {
        uint num = BitConverter.ToUInt32(buf, index);
        value += num * 0x85ebca77;
        value = RotateLeft(value, 13);
        value *= 0x9e3779b1;
        return value;
    }

    public uint GetHash(byte[] buf)
    {
        uint num;
        int index = 0;
        int length = buf.Length;
        if (length >= 0x10)
        {
            int num4 = length - 0x10;
            uint num5 = (this.seed + 0x9e3779b1) + 0x85ebca77;
            uint num6 = this.seed + 0x85ebca77;
            uint seed = this.seed;
            uint num8 = this.seed - 0x9e3779b1;
            do
            {
                num5 = CalcSubHash(num5, buf, index);
                index += 4;
                num6 = CalcSubHash(num6, buf, index);
                index += 4;
                seed = CalcSubHash(seed, buf, index);
                index += 4;
                num8 = CalcSubHash(num8, buf, index);
                index += 4;
            }
            while (index <= num4);
            num = ((RotateLeft(num5, 1) + RotateLeft(num6, 7)) + RotateLeft(seed, 12)) + RotateLeft(num8, 0x12);
        }
        else
        {
            num = this.seed + 0x165667b1;
        }
        num += (uint) length;
        while (index <= (length - 4))
        {
            num += BitConverter.ToUInt32(buf, index) * 0xc2b2ae3d;
            num = RotateLeft(num, 0x11) * 0x27d4eb2f;
            index += 4;
        }
        while (index < length)
        {
            num += (uint) (buf[index] * 0x165667b1);
            num = RotateLeft(num, 11) * 0x9e3779b1;
            index++;
        }
        num ^= num >> 15;
        num *= 0x85ebca77;
        num ^= num >> 13;
        num *= 0xc2b2ae3d;
        return (num ^ (num >> 0x10));
    }

    public override uint GetHash(params int[] buf)
    {
        uint num;
        int index = 0;
        int length = buf.Length;
        if (length >= 4)
        {
            int num4 = length - 4;
            uint num5 = (this.seed + 0x9e3779b1) + 0x85ebca77;
            uint num6 = this.seed + 0x85ebca77;
            uint seed = this.seed;
            uint num8 = this.seed - 0x9e3779b1;
            do
            {
                num5 = CalcSubHash(num5, (uint) buf[index]);
                index++;
                num6 = CalcSubHash(num6, (uint) buf[index]);
                index++;
                seed = CalcSubHash(seed, (uint) buf[index]);
                index++;
                num8 = CalcSubHash(num8, (uint) buf[index]);
                index++;
            }
            while (index <= num4);
            num = ((RotateLeft(num5, 1) + RotateLeft(num6, 7)) + RotateLeft(seed, 12)) + RotateLeft(num8, 0x12);
        }
        else
        {
            num = this.seed + 0x165667b1;
        }
        num += (uint) (length * 4);
        while (index < length)
        {
            num += (uint) (buf[index] * -1028477379);
            num = RotateLeft(num, 0x11) * 0x27d4eb2f;
            index++;
        }
        num ^= num >> 15;
        num *= 0x85ebca77;
        num ^= num >> 13;
        num *= 0xc2b2ae3d;
        return (num ^ (num >> 0x10));
    }

    public uint GetHash(params uint[] buf)
    {
        uint num;
        int index = 0;
        int length = buf.Length;
        if (length >= 4)
        {
            int num4 = length - 4;
            uint num5 = (this.seed + 0x9e3779b1) + 0x85ebca77;
            uint num6 = this.seed + 0x85ebca77;
            uint seed = this.seed;
            uint num8 = this.seed - 0x9e3779b1;
            do
            {
                num5 = CalcSubHash(num5, buf[index]);
                index++;
                num6 = CalcSubHash(num6, buf[index]);
                index++;
                seed = CalcSubHash(seed, buf[index]);
                index++;
                num8 = CalcSubHash(num8, buf[index]);
                index++;
            }
            while (index <= num4);
            num = ((RotateLeft(num5, 1) + RotateLeft(num6, 7)) + RotateLeft(seed, 12)) + RotateLeft(num8, 0x12);
        }
        else
        {
            num = this.seed + 0x165667b1;
        }
        num += (uint) (length * 4);
        while (index < length)
        {
            num += buf[index] * 0xc2b2ae3d;
            num = RotateLeft(num, 0x11) * 0x27d4eb2f;
            index++;
        }
        num ^= num >> 15;
        num *= 0x85ebca77;
        num ^= num >> 13;
        num *= 0xc2b2ae3d;
        return (num ^ (num >> 0x10));
    }

    public override uint GetHash(int buf)
    {
        uint num = this.seed + 0x165667b1;
        num += 4;
        num += (uint) (buf * -1028477379);
        num = RotateLeft(num, 0x11) * 0x27d4eb2f;
        num ^= num >> 15;
        num *= 0x85ebca77;
        num ^= num >> 13;
        num *= 0xc2b2ae3d;
        return (num ^ (num >> 0x10));
    }

    private static uint RotateLeft(uint value, int count)
    {
        return ((value << count) | (value >> (0x20 - count)));
    }
}

