namespace UnityTest
{
    using System;

    [Serializable]
    public abstract class ComparerBaseGeneric<T1, T2> : ComparerBase
    {
        public T2 constantValueGeneric;

        protected ComparerBaseGeneric()
        {
            this.constantValueGeneric = default(T2);
        }

        protected override bool Compare(object a, object b)
        {
            Type type = typeof(T2);
            if ((b == null) && ComparerBaseGeneric<T1, T2>.IsValueType(type))
            {
                throw new ArgumentException("Null was passed to a value-type argument");
            }
            return this.Compare((T1) a, (T2) b);
        }

        protected abstract bool Compare(T1 a, T2 b);
        public override Type[] GetAccepatbleTypesForA()
        {
            return new Type[] { typeof(T1) };
        }

        public override Type[] GetAccepatbleTypesForB()
        {
            return new Type[] { typeof(T2) };
        }

        public override object GetDefaultConstValue()
        {
            return default(T2);
        }

        private static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        public override object ConstValue
        {
            get
            {
                return this.constantValueGeneric;
            }
            set
            {
                this.constantValueGeneric = (T2) value;
            }
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

