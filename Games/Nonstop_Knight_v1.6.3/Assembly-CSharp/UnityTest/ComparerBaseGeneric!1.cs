namespace UnityTest
{
    using System;

    [Serializable]
    public abstract class ComparerBaseGeneric<T> : ComparerBaseGeneric<T, T>
    {
        protected ComparerBaseGeneric()
        {
        }
    }
}

