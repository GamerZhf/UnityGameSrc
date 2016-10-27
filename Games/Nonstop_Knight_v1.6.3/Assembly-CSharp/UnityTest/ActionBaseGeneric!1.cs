namespace UnityTest
{
    using System;

    public abstract class ActionBaseGeneric<T> : ActionBase
    {
        protected ActionBaseGeneric()
        {
        }

        protected override bool Compare(object objVal)
        {
            return this.Compare((T) objVal);
        }

        protected abstract bool Compare(T objVal);
        public override Type[] GetAccepatbleTypesForA()
        {
            return new Type[] { typeof(T) };
        }

        public override Type GetParameterType()
        {
            return typeof(T);
        }

        protected override bool UseCache
        {
            get
            {
                return true;
            }
        }
    }
}

