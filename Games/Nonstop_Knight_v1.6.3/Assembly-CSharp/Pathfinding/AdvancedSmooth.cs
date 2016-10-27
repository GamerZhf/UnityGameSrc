namespace Pathfinding
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [Serializable, AddComponentMenu("Pathfinding/Modifiers/Advanced Smooth"), HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_advanced_smooth.php")]
    public class AdvancedSmooth : MonoModifier
    {
        public MaxTurn turnConstruct1 = new MaxTurn();
        public ConstantTurn turnConstruct2 = new ConstantTurn();
        public float turningRadius = 1f;

        public override void Apply(Path p)
        {
            Vector3[] vectorPath = p.vectorPath.ToArray();
            if ((vectorPath != null) && (vectorPath.Length > 2))
            {
                List<Vector3> output = new List<Vector3>();
                output.Add(vectorPath[0]);
                TurnConstructor.turningRadius = this.turningRadius;
                for (int i = 1; i < (vectorPath.Length - 1); i++)
                {
                    List<Turn> turnList = new List<Turn>();
                    TurnConstructor.Setup(i, vectorPath);
                    this.turnConstruct1.Prepare(i, vectorPath);
                    this.turnConstruct2.Prepare(i, vectorPath);
                    TurnConstructor.PostPrepare();
                    if (i == 1)
                    {
                        this.turnConstruct1.PointToTangent(turnList);
                        this.turnConstruct2.PointToTangent(turnList);
                    }
                    else
                    {
                        this.turnConstruct1.TangentToTangent(turnList);
                        this.turnConstruct2.TangentToTangent(turnList);
                    }
                    this.EvaluatePaths(turnList, output);
                    if (i == (vectorPath.Length - 2))
                    {
                        this.turnConstruct1.TangentToPoint(turnList);
                        this.turnConstruct2.TangentToPoint(turnList);
                    }
                    this.EvaluatePaths(turnList, output);
                }
                output.Add(vectorPath[vectorPath.Length - 1]);
                p.vectorPath = output;
            }
        }

        private void EvaluatePaths(List<Turn> turnList, List<Vector3> output)
        {
            turnList.Sort();
            for (int i = 0; i < turnList.Count; i++)
            {
                if (i == 0)
                {
                    turnList[i].GetPath(output);
                }
            }
            turnList.Clear();
            if (TurnConstructor.changedPreviousTangent)
            {
                this.turnConstruct1.OnTangentUpdate();
                this.turnConstruct2.OnTangentUpdate();
            }
        }

        public override int Order
        {
            get
            {
                return 40;
            }
        }

        [Serializable]
        public class ConstantTurn : AdvancedSmooth.TurnConstructor
        {
            private Vector3 circleCenter;
            private bool clockwise;
            private double gamma1;
            private double gamma2;

            public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
            {
                Vector3 vector = this.circleCenter - AdvancedSmooth.TurnConstructor.current;
                base.AddCircleSegment(this.gamma1, this.gamma2, this.clockwise, this.circleCenter, output, vector.magnitude);
                Vector3 vector2 = AdvancedSmooth.TurnConstructor.current - this.circleCenter;
                AdvancedSmooth.TurnConstructor.normal = vector2.normalized;
                AdvancedSmooth.TurnConstructor.t2 = Vector3.Cross(AdvancedSmooth.TurnConstructor.normal, Vector3.up).normalized;
                AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
                if (!this.clockwise)
                {
                    AdvancedSmooth.TurnConstructor.t2 = -AdvancedSmooth.TurnConstructor.t2;
                    AdvancedSmooth.TurnConstructor.normal = -AdvancedSmooth.TurnConstructor.normal;
                }
                AdvancedSmooth.TurnConstructor.changedPreviousTangent = true;
            }

            public override void Prepare(int i, Vector3[] vectorPath)
            {
            }

            public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
            {
                bool flag;
                Vector3 vector = Vector3.Cross(AdvancedSmooth.TurnConstructor.t1, Vector3.up);
                Vector3 lhs = AdvancedSmooth.TurnConstructor.current - AdvancedSmooth.TurnConstructor.prev;
                Vector3 vector3 = ((Vector3) (lhs * 0.5f)) + AdvancedSmooth.TurnConstructor.prev;
                lhs = Vector3.Cross(lhs, Vector3.up);
                this.circleCenter = VectorMath.LineDirIntersectionPointXZ(AdvancedSmooth.TurnConstructor.prev, vector, vector3, lhs, out flag);
                if (flag)
                {
                    this.gamma1 = base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.circleCenter);
                    this.gamma2 = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.circleCenter);
                    this.clockwise = !VectorMath.RightOrColinearXZ(this.circleCenter, AdvancedSmooth.TurnConstructor.prev, AdvancedSmooth.TurnConstructor.prev + AdvancedSmooth.TurnConstructor.t1);
                    double angle = !this.clockwise ? base.CounterClockwiseAngle(this.gamma1, this.gamma2) : base.ClockwiseAngle(this.gamma1, this.gamma2);
                    Vector3 vector4 = this.circleCenter - AdvancedSmooth.TurnConstructor.current;
                    angle = base.GetLengthFromAngle(angle, (double) vector4.magnitude);
                    turnList.Add(new AdvancedSmooth.Turn((float) angle, this, 0));
                }
            }
        }

        [Serializable]
        public class MaxTurn : AdvancedSmooth.TurnConstructor
        {
            private double alfaLeftLeft;
            private double alfaLeftRight;
            private double alfaRightLeft;
            private double alfaRightRight;
            private double betaLeftLeft;
            private double betaLeftRight;
            private double betaRightLeft;
            private double betaRightRight;
            private double deltaLeftRight;
            private double deltaRightLeft;
            private double gammaLeft;
            private double gammaRight;
            private Vector3 leftCircleCenter;
            private Vector3 preLeftCircleCenter = Vector3.zero;
            private Vector3 preRightCircleCenter = Vector3.zero;
            private double preVaLeft;
            private double preVaRight;
            private Vector3 rightCircleCenter;
            private double vaLeft;
            private double vaRight;

            public override void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output)
            {
                switch (turn.id)
                {
                    case 0:
                        base.AddCircleSegment(this.gammaRight, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 1:
                        base.AddCircleSegment(this.gammaLeft, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 2:
                        base.AddCircleSegment(this.preVaRight, this.alfaRightRight - 1.5707963267948966, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        base.AddCircleSegment(this.alfaRightRight - 1.5707963267948966, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 3:
                        base.AddCircleSegment(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft, true, this.preRightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        base.AddCircleSegment((this.alfaRightLeft - this.deltaRightLeft) + 3.1415926535897931, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 4:
                        base.AddCircleSegment(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        base.AddCircleSegment((this.alfaLeftRight + this.deltaLeftRight) + 3.1415926535897931, this.vaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 5:
                        base.AddCircleSegment(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966, false, this.preLeftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        base.AddCircleSegment(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 6:
                        base.AddCircleSegment(this.vaRight, this.gammaRight, true, this.rightCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;

                    case 7:
                        base.AddCircleSegment(this.vaLeft, this.gammaLeft, false, this.leftCircleCenter, output, AdvancedSmooth.TurnConstructor.turningRadius);
                        break;
                }
            }

            public override void OnTangentUpdate()
            {
                this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + ((Vector3) (AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius));
                this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - ((Vector3) (AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius));
                this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
                this.vaLeft = this.vaRight + 3.1415926535897931;
            }

            public override void PointToTangent(List<AdvancedSmooth.Turn> turnList)
            {
                bool flag = false;
                bool flag2 = false;
                Vector3 vector = AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter;
                float magnitude = vector.magnitude;
                Vector3 vector2 = AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter;
                float num2 = vector2.magnitude;
                if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
                {
                    flag = true;
                }
                if (num2 < AdvancedSmooth.TurnConstructor.turningRadius)
                {
                    flag2 = true;
                }
                double num3 = !flag ? base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter) : 0.0;
                double num4 = !flag ? (1.5707963267948966 - Math.Asin((double) (AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.rightCircleCenter).magnitude))) : 0.0;
                this.gammaRight = num3 + num4;
                double num5 = !flag ? base.ClockwiseAngle(this.gammaRight, this.vaRight) : 0.0;
                double num6 = !flag2 ? base.Atan2(AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter) : 0.0;
                double num7 = !flag2 ? (1.5707963267948966 - Math.Asin((double) (AdvancedSmooth.TurnConstructor.turningRadius / (AdvancedSmooth.TurnConstructor.prev - this.leftCircleCenter).magnitude))) : 0.0;
                this.gammaLeft = num6 - num7;
                double num8 = !flag2 ? base.CounterClockwiseAngle(this.gammaLeft, this.vaLeft) : 0.0;
                if (!flag)
                {
                    turnList.Add(new AdvancedSmooth.Turn((float) num5, this, 0));
                }
                if (!flag2)
                {
                    turnList.Add(new AdvancedSmooth.Turn((float) num8, this, 1));
                }
            }

            public override void Prepare(int i, Vector3[] vectorPath)
            {
                this.preRightCircleCenter = this.rightCircleCenter;
                this.preLeftCircleCenter = this.leftCircleCenter;
                this.rightCircleCenter = AdvancedSmooth.TurnConstructor.current + ((Vector3) (AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius));
                this.leftCircleCenter = AdvancedSmooth.TurnConstructor.current - ((Vector3) (AdvancedSmooth.TurnConstructor.normal * AdvancedSmooth.TurnConstructor.turningRadius));
                this.preVaRight = this.vaRight;
                this.preVaLeft = this.vaLeft;
                this.vaRight = base.Atan2(AdvancedSmooth.TurnConstructor.current - this.rightCircleCenter);
                this.vaLeft = this.vaRight + 3.1415926535897931;
            }

            public override void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
            {
                bool flag = false;
                bool flag2 = false;
                Vector3 vector = AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter;
                float magnitude = vector.magnitude;
                Vector3 vector2 = AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter;
                float num2 = vector2.magnitude;
                if (magnitude < AdvancedSmooth.TurnConstructor.turningRadius)
                {
                    flag = true;
                }
                if (num2 < AdvancedSmooth.TurnConstructor.turningRadius)
                {
                    flag2 = true;
                }
                if (!flag)
                {
                    double num3 = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.rightCircleCenter);
                    double num4 = 1.5707963267948966 - Math.Asin((double) (AdvancedSmooth.TurnConstructor.turningRadius / magnitude));
                    this.gammaRight = num3 - num4;
                    double num5 = base.ClockwiseAngle(this.vaRight, this.gammaRight);
                    turnList.Add(new AdvancedSmooth.Turn((float) num5, this, 6));
                }
                if (!flag2)
                {
                    double num6 = base.Atan2(AdvancedSmooth.TurnConstructor.next - this.leftCircleCenter);
                    double num7 = 1.5707963267948966 - Math.Asin((double) (AdvancedSmooth.TurnConstructor.turningRadius / num2));
                    this.gammaLeft = num6 + num7;
                    double num8 = base.CounterClockwiseAngle(this.vaLeft, this.gammaLeft);
                    turnList.Add(new AdvancedSmooth.Turn((float) num8, this, 7));
                }
            }

            public override void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
            {
                this.alfaRightRight = base.Atan2(this.rightCircleCenter - this.preRightCircleCenter);
                this.alfaLeftLeft = base.Atan2(this.leftCircleCenter - this.preLeftCircleCenter);
                this.alfaRightLeft = base.Atan2(this.leftCircleCenter - this.preRightCircleCenter);
                this.alfaLeftRight = base.Atan2(this.rightCircleCenter - this.preLeftCircleCenter);
                Vector3 vector9 = this.leftCircleCenter - this.preRightCircleCenter;
                double magnitude = vector9.magnitude;
                Vector3 vector10 = this.rightCircleCenter - this.preLeftCircleCenter;
                double num2 = vector10.magnitude;
                bool flag = false;
                bool flag2 = false;
                if (magnitude < (AdvancedSmooth.TurnConstructor.turningRadius * 2f))
                {
                    magnitude = AdvancedSmooth.TurnConstructor.turningRadius * 2f;
                    flag = true;
                }
                if (num2 < (AdvancedSmooth.TurnConstructor.turningRadius * 2f))
                {
                    num2 = AdvancedSmooth.TurnConstructor.turningRadius * 2f;
                    flag2 = true;
                }
                this.deltaRightLeft = !flag ? (1.5707963267948966 - Math.Asin(((double) (AdvancedSmooth.TurnConstructor.turningRadius * 2f)) / magnitude)) : 0.0;
                this.deltaLeftRight = !flag2 ? (1.5707963267948966 - Math.Asin(((double) (AdvancedSmooth.TurnConstructor.turningRadius * 2f)) / num2)) : 0.0;
                this.betaRightRight = base.ClockwiseAngle(this.preVaRight, this.alfaRightRight - 1.5707963267948966);
                this.betaRightLeft = base.ClockwiseAngle(this.preVaRight, this.alfaRightLeft - this.deltaRightLeft);
                this.betaLeftRight = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftRight + this.deltaLeftRight);
                this.betaLeftLeft = base.CounterClockwiseAngle(this.preVaLeft, this.alfaLeftLeft + 1.5707963267948966);
                this.betaRightRight += base.ClockwiseAngle(this.alfaRightRight - 1.5707963267948966, this.vaRight);
                this.betaRightLeft += base.CounterClockwiseAngle(this.alfaRightLeft + this.deltaRightLeft, this.vaLeft);
                this.betaLeftRight += base.ClockwiseAngle(this.alfaLeftRight - this.deltaLeftRight, this.vaRight);
                this.betaLeftLeft += base.CounterClockwiseAngle(this.alfaLeftLeft + 1.5707963267948966, this.vaLeft);
                this.betaRightRight = base.GetLengthFromAngle(this.betaRightRight, (double) AdvancedSmooth.TurnConstructor.turningRadius);
                this.betaRightLeft = base.GetLengthFromAngle(this.betaRightLeft, (double) AdvancedSmooth.TurnConstructor.turningRadius);
                this.betaLeftRight = base.GetLengthFromAngle(this.betaLeftRight, (double) AdvancedSmooth.TurnConstructor.turningRadius);
                this.betaLeftLeft = base.GetLengthFromAngle(this.betaLeftLeft, (double) AdvancedSmooth.TurnConstructor.turningRadius);
                Vector3 vector = ((Vector3) (base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.preRightCircleCenter;
                Vector3 vector3 = ((Vector3) (base.AngleToVector(this.alfaRightLeft - this.deltaRightLeft) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.preRightCircleCenter;
                Vector3 vector5 = ((Vector3) (base.AngleToVector(this.alfaLeftRight + this.deltaLeftRight) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.preLeftCircleCenter;
                Vector3 vector7 = ((Vector3) (base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.preLeftCircleCenter;
                Vector3 vector2 = ((Vector3) (base.AngleToVector(this.alfaRightRight - 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.rightCircleCenter;
                Vector3 vector4 = ((Vector3) (base.AngleToVector((this.alfaRightLeft - this.deltaRightLeft) + 3.1415926535897931) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.leftCircleCenter;
                Vector3 vector6 = ((Vector3) (base.AngleToVector((this.alfaLeftRight + this.deltaLeftRight) + 3.1415926535897931) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.rightCircleCenter;
                Vector3 vector8 = ((Vector3) (base.AngleToVector(this.alfaLeftLeft + 1.5707963267948966) * AdvancedSmooth.TurnConstructor.turningRadius)) + this.leftCircleCenter;
                Vector3 vector11 = vector - vector2;
                this.betaRightRight += vector11.magnitude;
                Vector3 vector12 = vector3 - vector4;
                this.betaRightLeft += vector12.magnitude;
                Vector3 vector13 = vector5 - vector6;
                this.betaLeftRight += vector13.magnitude;
                Vector3 vector14 = vector7 - vector8;
                this.betaLeftLeft += vector14.magnitude;
                if (flag)
                {
                    this.betaRightLeft += 10000000.0;
                }
                if (flag2)
                {
                    this.betaLeftRight += 10000000.0;
                }
                turnList.Add(new AdvancedSmooth.Turn((float) this.betaRightRight, this, 2));
                turnList.Add(new AdvancedSmooth.Turn((float) this.betaRightLeft, this, 3));
                turnList.Add(new AdvancedSmooth.Turn((float) this.betaLeftRight, this, 4));
                turnList.Add(new AdvancedSmooth.Turn((float) this.betaLeftLeft, this, 5));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Turn : IComparable<AdvancedSmooth.Turn>
        {
            public float length;
            public int id;
            public AdvancedSmooth.TurnConstructor constructor;
            public Turn(float length, AdvancedSmooth.TurnConstructor constructor, [Optional, DefaultParameterValue(0)] int id)
            {
                this.length = length;
                this.id = id;
                this.constructor = constructor;
            }

            public float score
            {
                get
                {
                    return ((this.length * this.constructor.factorBias) + this.constructor.constantBias);
                }
            }
            public void GetPath(List<Vector3> output)
            {
                this.constructor.GetPath(this, output);
            }

            public int CompareTo(AdvancedSmooth.Turn t)
            {
                return ((t.score <= this.score) ? ((t.score >= this.score) ? 0 : 1) : -1);
            }

            public static bool operator <(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
            {
                return (lhs.score < rhs.score);
            }

            public static bool operator >(AdvancedSmooth.Turn lhs, AdvancedSmooth.Turn rhs)
            {
                return (lhs.score > rhs.score);
            }
        }

        public abstract class TurnConstructor
        {
            public static bool changedPreviousTangent;
            public float constantBias;
            public static Vector3 current;
            public float factorBias = 1f;
            public static Vector3 next;
            public static Vector3 normal;
            public static Vector3 prev;
            public static Vector3 prevNormal;
            public static Vector3 t1;
            public static Vector3 t2;
            public const double ThreeSixtyRadians = 6.2831853071795862;
            public static float turningRadius = 1f;

            protected TurnConstructor()
            {
            }

            public void AddCircleSegment(double startAngle, double endAngle, bool clockwise, Vector3 center, List<Vector3> output, float radius)
            {
                double num = 0.062831853071795868;
                if (clockwise)
                {
                    while (endAngle > (startAngle + 6.2831853071795862))
                    {
                        endAngle -= 6.2831853071795862;
                    }
                    while (endAngle < startAngle)
                    {
                        endAngle += 6.2831853071795862;
                    }
                }
                else
                {
                    while (endAngle < (startAngle - 6.2831853071795862))
                    {
                        endAngle += 6.2831853071795862;
                    }
                    while (endAngle > startAngle)
                    {
                        endAngle -= 6.2831853071795862;
                    }
                }
                if (clockwise)
                {
                    for (double i = startAngle; i < endAngle; i += num)
                    {
                        output.Add(((Vector3) (this.AngleToVector(i) * radius)) + center);
                    }
                }
                else
                {
                    for (double j = startAngle; j > endAngle; j -= num)
                    {
                        output.Add(((Vector3) (this.AngleToVector(j) * radius)) + center);
                    }
                }
                output.Add(((Vector3) (this.AngleToVector(endAngle) * radius)) + center);
            }

            public Vector3 AngleToVector(double a)
            {
                return new Vector3((float) Math.Cos(a), 0f, (float) Math.Sin(a));
            }

            public double Atan2(Vector3 v)
            {
                return Math.Atan2((double) v.z, (double) v.x);
            }

            public double ClampAngle(double a)
            {
                while (a < 0.0)
                {
                    a += 6.2831853071795862;
                }
                while (a > 6.2831853071795862)
                {
                    a -= 6.2831853071795862;
                }
                return a;
            }

            public double ClockwiseAngle(double from, double to)
            {
                return this.ClampAngle(to - from);
            }

            public double CounterClockwiseAngle(double from, double to)
            {
                return this.ClampAngle(from - to);
            }

            public void DebugCircle(Vector3 center, double radius, Color color)
            {
                double num = 0.062831853071795868;
                Vector3 start = ((Vector3) (this.AngleToVector(-num) * ((float) radius))) + center;
                for (double i = 0.0; i < 6.2831853071795862; i += num)
                {
                    Vector3 end = ((Vector3) (this.AngleToVector(i) * ((float) radius))) + center;
                    Debug.DrawLine(start, end, color);
                    start = end;
                }
            }

            public void DebugCircleSegment(Vector3 center, double startAngle, double endAngle, double radius, Color color)
            {
                double num = 0.062831853071795868;
                while (endAngle < startAngle)
                {
                    endAngle += 6.2831853071795862;
                }
                Vector3 start = ((Vector3) (this.AngleToVector(startAngle) * ((float) radius))) + center;
                for (double i = startAngle + num; i < endAngle; i += num)
                {
                    Debug.DrawLine(start, ((Vector3) (this.AngleToVector(i) * ((float) radius))) + center);
                }
                Debug.DrawLine(start, ((Vector3) (this.AngleToVector(endAngle) * ((float) radius))) + center);
            }

            public double GetLengthFromAngle(double angle, double radius)
            {
                return (radius * angle);
            }

            public abstract void GetPath(AdvancedSmooth.Turn turn, List<Vector3> output);
            public virtual void OnTangentUpdate()
            {
            }

            public virtual void PointToTangent(List<AdvancedSmooth.Turn> turnList)
            {
            }

            public static void PostPrepare()
            {
                changedPreviousTangent = false;
            }

            public abstract void Prepare(int i, Vector3[] vectorPath);
            public static void Setup(int i, Vector3[] vectorPath)
            {
                current = vectorPath[i];
                prev = vectorPath[i - 1];
                next = vectorPath[i + 1];
                prev.y = current.y;
                next.y = current.y;
                t1 = t2;
                Vector3 vector = next - current;
                Vector3 vector2 = prev - current;
                t2 = vector.normalized - vector2.normalized;
                t2 = t2.normalized;
                prevNormal = normal;
                normal = Vector3.Cross(t2, Vector3.up);
                normal = normal.normalized;
            }

            public virtual void TangentToPoint(List<AdvancedSmooth.Turn> turnList)
            {
            }

            public virtual void TangentToTangent(List<AdvancedSmooth.Turn> turnList)
            {
            }

            public double ToDegrees(double rad)
            {
                return (rad * 57.295780181884766);
            }
        }
    }
}

