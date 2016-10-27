namespace Pathfinding
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct Int3
    {
        public const int Precision = 0x3e8;
        public const float FloatPrecision = 1000f;
        public const float PrecisionFactor = 0.001f;
        public int x;
        public int y;
        public int z;
        public Int3(Vector3 position)
        {
            this.x = (int) Math.Round((double) (position.x * 1000f));
            this.y = (int) Math.Round((double) (position.y * 1000f));
            this.z = (int) Math.Round((double) (position.z * 1000f));
        }

        public Int3(int _x, int _y, int _z)
        {
            this.x = _x;
            this.y = _y;
            this.z = _z;
        }

        public static Int3 zero
        {
            get
            {
                return new Int3();
            }
        }
        public int this[int i]
        {
            get
            {
                return ((i != 0) ? ((i != 1) ? this.z : this.y) : this.x);
            }
            set
            {
                if (i == 0)
                {
                    this.x = value;
                }
                else if (i == 1)
                {
                    this.y = value;
                }
                else
                {
                    this.z = value;
                }
            }
        }
        public static float Angle(Int3 lhs, Int3 rhs)
        {
            double d = ((double) Dot(lhs, rhs)) / (lhs.magnitude * rhs.magnitude);
            d = (d >= -1.0) ? ((d <= 1.0) ? d : 1.0) : -1.0;
            return (float) Math.Acos(d);
        }

        public static int Dot(Int3 lhs, Int3 rhs)
        {
            return (((lhs.x * rhs.x) + (lhs.y * rhs.y)) + (lhs.z * rhs.z));
        }

        public static long DotLong(Int3 lhs, Int3 rhs)
        {
            return (((lhs.x * rhs.x) + (lhs.y * rhs.y)) + (lhs.z * rhs.z));
        }

        public Int3 Normal2D()
        {
            return new Int3(this.z, this.y, -this.x);
        }

        public float magnitude
        {
            get
            {
                double x = this.x;
                double y = this.y;
                double z = this.z;
                return (float) Math.Sqrt(((x * x) + (y * y)) + (z * z));
            }
        }
        public int costMagnitude
        {
            get
            {
                return (int) Math.Round((double) this.magnitude);
            }
        }
        [Obsolete("This property is deprecated. Use magnitude or cast to a Vector3")]
        public float worldMagnitude
        {
            get
            {
                double x = this.x;
                double y = this.y;
                double z = this.z;
                return (((float) Math.Sqrt(((x * x) + (y * y)) + (z * z))) * 0.001f);
            }
        }
        public float sqrMagnitude
        {
            get
            {
                double x = this.x;
                double y = this.y;
                double z = this.z;
                return (float) (((x * x) + (y * y)) + (z * z));
            }
        }
        public long sqrMagnitudeLong
        {
            get
            {
                long x = this.x;
                long y = this.y;
                long z = this.z;
                return (((x * x) + (y * y)) + (z * z));
            }
        }
        public override string ToString()
        {
            object[] objArray1 = new object[] { "( ", this.x, ", ", this.y, ", ", this.z, ")" };
            return string.Concat(objArray1);
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }
            Int3 num = (Int3) o;
            return (((this.x == num.x) && (this.y == num.y)) && (this.z == num.z));
        }

        public override int GetHashCode()
        {
            return (((this.x * 0x466f45d) ^ (this.y * 0x127409f)) ^ (this.z * 0x4f9ffb7));
        }

        public static bool operator ==(Int3 lhs, Int3 rhs)
        {
            return (((lhs.x == rhs.x) && (lhs.y == rhs.y)) && (lhs.z == rhs.z));
        }

        public static bool operator !=(Int3 lhs, Int3 rhs)
        {
            return (((lhs.x != rhs.x) || (lhs.y != rhs.y)) || (lhs.z != rhs.z));
        }

        public static explicit operator Int3(Vector3 ob)
        {
            return new Int3((int) Math.Round((double) (ob.x * 1000f)), (int) Math.Round((double) (ob.y * 1000f)), (int) Math.Round((double) (ob.z * 1000f)));
        }

        public static explicit operator Vector3(Int3 ob)
        {
            return new Vector3(ob.x * 0.001f, ob.y * 0.001f, ob.z * 0.001f);
        }

        public static Int3 operator -(Int3 lhs, Int3 rhs)
        {
            lhs.x -= rhs.x;
            lhs.y -= rhs.y;
            lhs.z -= rhs.z;
            return lhs;
        }

        public static Int3 operator -(Int3 lhs)
        {
            lhs.x = -lhs.x;
            lhs.y = -lhs.y;
            lhs.z = -lhs.z;
            return lhs;
        }

        public static Int3 operator +(Int3 lhs, Int3 rhs)
        {
            lhs.x += rhs.x;
            lhs.y += rhs.y;
            lhs.z += rhs.z;
            return lhs;
        }

        public static Int3 operator *(Int3 lhs, int rhs)
        {
            lhs.x *= rhs;
            lhs.y *= rhs;
            lhs.z *= rhs;
            return lhs;
        }

        public static Int3 operator *(Int3 lhs, float rhs)
        {
            lhs.x = (int) Math.Round((double) (lhs.x * rhs));
            lhs.y = (int) Math.Round((double) (lhs.y * rhs));
            lhs.z = (int) Math.Round((double) (lhs.z * rhs));
            return lhs;
        }

        public static Int3 operator *(Int3 lhs, double rhs)
        {
            lhs.x = (int) Math.Round((double) (lhs.x * rhs));
            lhs.y = (int) Math.Round((double) (lhs.y * rhs));
            lhs.z = (int) Math.Round((double) (lhs.z * rhs));
            return lhs;
        }

        public static Int3 operator /(Int3 lhs, float rhs)
        {
            lhs.x = (int) Math.Round((double) (((float) lhs.x) / rhs));
            lhs.y = (int) Math.Round((double) (((float) lhs.y) / rhs));
            lhs.z = (int) Math.Round((double) (((float) lhs.z) / rhs));
            return lhs;
        }

        public static implicit operator string(Int3 ob)
        {
            return ob.ToString();
        }
    }
}

