namespace UnityTest
{
    using System;
    using UnityEngine;

    public class Vector4Comparer : VectorComparerBase<Vector4>
    {
        public CompareType compareType;
        public double floatingPointError;

        protected override bool Compare(Vector4 a, Vector4 b)
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

        public override int GetDepthOfSearch()
        {
            return 3;
        }

        public enum CompareType
        {
            MagnitudeEquals,
            MagnitudeNotEquals
        }
    }
}

