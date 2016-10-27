namespace UnityTest
{
    using System;

    public abstract class VectorComparerBase<T> : ComparerBaseGeneric<T>
    {
        protected VectorComparerBase()
        {
        }

        protected bool AreVectorMagnitudeEqual(float a, float b, double floatingPointError)
        {
            return (((Math.Abs(a) < floatingPointError) && (Math.Abs(b) < floatingPointError)) || (Math.Abs((float) (a - b)) < floatingPointError));
        }
    }
}

