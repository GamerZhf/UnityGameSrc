namespace UnityTest
{
    using System;
    using UnityEngine;

    public class Vector2Comparer : VectorComparerBase<Vector2>
    {
        public CompareType compareType;
        public float floatingPointError = 0.0001f;

        protected override bool Compare(Vector2 a, Vector2 b)
        {
            CompareType compareType = this.compareType;
            if (compareType != CompareType.MagnitudeEquals)
            {
                if (compareType != CompareType.MagnitudeNotEquals)
                {
                    throw new Exception();
                }
                return !base.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, (double) this.floatingPointError);
            }
            return base.AreVectorMagnitudeEqual(a.magnitude, b.magnitude, (double) this.floatingPointError);
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

