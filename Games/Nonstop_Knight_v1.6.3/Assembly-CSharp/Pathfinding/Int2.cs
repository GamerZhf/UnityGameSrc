namespace Pathfinding
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Int2
    {
        public int x;
        public int y;
        private static readonly int[] Rotations;
        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        static Int2()
        {
            Rotations = new int[] { 1, 0, 0, 1, 0, 1, -1, 0, -1, 0, 0, -1, 0, -1, 1, 0 };
        }

        public long sqrMagnitudeLong
        {
            get
            {
                return ((this.x * this.x) + (this.y * this.y));
            }
        }
        public static long DotLong(Int2 a, Int2 b)
        {
            return ((a.x * b.x) + (a.y * b.y));
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }
            Int2 num = (Int2) o;
            return ((this.x == num.x) && (this.y == num.y));
        }

        public override int GetHashCode()
        {
            return ((this.x * 0xc005) + (this.y * 0x1800d));
        }

        [Obsolete("Deprecated becuase it is not used by any part of the A* Pathfinding Project")]
        public static Int2 Rotate(Int2 v, int r)
        {
            r = r % 4;
            return new Int2((v.x * Rotations[r * 4]) + (v.y * Rotations[(r * 4) + 1]), (v.x * Rotations[(r * 4) + 2]) + (v.y * Rotations[(r * 4) + 3]));
        }

        public static Int2 Min(Int2 a, Int2 b)
        {
            return new Int2(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        }

        public static Int2 Max(Int2 a, Int2 b)
        {
            return new Int2(Math.Max(a.x, b.x), Math.Max(a.y, b.y));
        }

        public static Int2 FromInt3XZ(Int3 o)
        {
            return new Int2(o.x, o.z);
        }

        public static Int3 ToInt3XZ(Int2 o)
        {
            return new Int3(o.x, 0, o.y);
        }

        public override string ToString()
        {
            object[] objArray1 = new object[] { "(", this.x, ", ", this.y, ")" };
            return string.Concat(objArray1);
        }

        public static Int2 operator +(Int2 a, Int2 b)
        {
            return new Int2(a.x + b.x, a.y + b.y);
        }

        public static Int2 operator -(Int2 a, Int2 b)
        {
            return new Int2(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Int2 a, Int2 b)
        {
            return ((a.x == b.x) && (a.y == b.y));
        }

        public static bool operator !=(Int2 a, Int2 b)
        {
            return ((a.x != b.x) || (a.y != b.y));
        }
    }
}

