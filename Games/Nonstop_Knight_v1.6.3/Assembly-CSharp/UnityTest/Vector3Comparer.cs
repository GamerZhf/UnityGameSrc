namespace UnityTest
{
    using System;
    using UnityEngine;

    public class Vector3Comparer : VectorComparerBase<Vector3>
    {
        public CompareType compareType;
        public double floatingPointError = 9.9999997473787516E-05;

        protected override bool Compare(Vector3 a, Vector3 b)
        {
            CompareType compareType = this.compareType;
            if (compareType != CompareType.MagnitudeEquals)
            {
                if (compareType != CompareType.MagnitudeNotEquals)
                {
                    throw new Exception();
                }
                return !base.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, this.floatingPointError);
            }
            return base.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, this.floatingPointError);
        }

        public enum CompareType
        {
            MagnitudeEquals,
            MagnitudeNotEquals
        }
    }
}

