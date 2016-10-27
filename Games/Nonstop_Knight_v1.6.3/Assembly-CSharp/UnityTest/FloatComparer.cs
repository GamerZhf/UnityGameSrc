namespace UnityTest
{
    using System;

    public class FloatComparer : ComparerBaseGeneric<float>
    {
        public CompareTypes compareTypes;
        public double floatingPointError = 9.9999997473787516E-05;

        protected override bool Compare(float a, float b)
        {
            switch (this.compareTypes)
            {
                case CompareTypes.Equal:
                    return (Math.Abs((float) (a - b)) < this.floatingPointError);

                case CompareTypes.NotEqual:
                    return (Math.Abs((float) (a - b)) > this.floatingPointError);

                case CompareTypes.Greater:
                    return (a > b);

                case CompareTypes.Less:
                    return (a < b);
            }
            throw new Exception();
        }

        public override int GetDepthOfSearch()
        {
            return 3;
        }

        public enum CompareTypes
        {
            Equal,
            NotEqual,
            Greater,
            Less
        }
    }
}

