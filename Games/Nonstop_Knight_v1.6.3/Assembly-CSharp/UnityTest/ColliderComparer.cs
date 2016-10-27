namespace UnityTest
{
    using System;
    using UnityEngine;

    public class ColliderComparer : ComparerBaseGeneric<Bounds>
    {
        public CompareType compareType;

        protected override bool Compare(Bounds a, Bounds b)
        {
            CompareType compareType = this.compareType;
            if (compareType != CompareType.Intersects)
            {
                if (compareType != CompareType.DoesNotIntersect)
                {
                    throw new Exception();
                }
                return !a.Intersects(b);
            }
            return a.Intersects(b);
        }

        public enum CompareType
        {
            Intersects,
            DoesNotIntersect
        }
    }
}

