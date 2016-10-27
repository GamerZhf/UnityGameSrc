namespace UnityTest
{
    using System;
    using UnityEngine;

    public class TransformComparer : ComparerBaseGeneric<Transform>
    {
        public CompareType compareType;

        protected override bool Compare(Transform a, Transform b)
        {
            if (this.compareType == CompareType.Equals)
            {
                return (a.position == b.position);
            }
            if (this.compareType != CompareType.NotEquals)
            {
                throw new Exception();
            }
            return (a.position != b.position);
        }

        public enum CompareType
        {
            Equals,
            NotEquals
        }
    }
}

