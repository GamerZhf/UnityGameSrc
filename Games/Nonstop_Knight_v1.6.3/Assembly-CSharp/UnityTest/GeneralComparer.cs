namespace UnityTest
{
    using System;

    public class GeneralComparer : ComparerBase
    {
        public CompareType compareType;

        protected override bool Compare(object a, object b)
        {
            if (this.compareType == CompareType.AEqualsB)
            {
                return a.Equals(b);
            }
            if (this.compareType != CompareType.ANotEqualsB)
            {
                throw new Exception();
            }
            return !a.Equals(b);
        }

        public enum CompareType
        {
            AEqualsB,
            ANotEqualsB
        }
    }
}

