namespace UnityTest
{
    using System;

    public class StringComparer : ComparerBaseGeneric<string>
    {
        public CompareType compareType;
        public StringComparison comparisonType = StringComparison.Ordinal;
        public bool ignoreCase;

        protected override bool Compare(string a, string b)
        {
            if (this.ignoreCase)
            {
                a = a.ToLower();
                b = b.ToLower();
            }
            switch (this.compareType)
            {
                case CompareType.Equal:
                    return (string.Compare(a, b, this.comparisonType) == 0);

                case CompareType.NotEqual:
                    return (string.Compare(a, b, this.comparisonType) != 0);

                case CompareType.Shorter:
                    return (string.Compare(a, b, this.comparisonType) < 0);

                case CompareType.Longer:
                    return (string.Compare(a, b, this.comparisonType) > 0);
            }
            throw new Exception();
        }

        public enum CompareType
        {
            Equal,
            NotEqual,
            Shorter,
            Longer
        }
    }
}

