namespace UnityTest
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class ComparerBase : ActionBase
    {
        [CompilerGenerated]
        private object <ConstValue>k__BackingField;
        public CompareToType compareToType;
        private MemberResolver m_MemberResolverB;
        protected object m_ObjOtherVal;
        public GameObject other;
        public string otherPropertyPath = string.Empty;

        protected ComparerBase()
        {
        }

        protected override bool Compare(object objValue)
        {
            if (this.compareToType == CompareToType.CompareToConstantValue)
            {
                this.m_ObjOtherVal = this.ConstValue;
            }
            else if (this.compareToType == CompareToType.CompareToNull)
            {
                this.m_ObjOtherVal = null;
            }
            else if (this.other == null)
            {
                this.m_ObjOtherVal = null;
            }
            else
            {
                if (this.m_MemberResolverB == null)
                {
                    this.m_MemberResolverB = new MemberResolver(this.other, this.otherPropertyPath);
                }
                this.m_ObjOtherVal = this.m_MemberResolverB.GetValue(this.UseCache);
            }
            return this.Compare(objValue, this.m_ObjOtherVal);
        }

        protected abstract bool Compare(object a, object b);
        public virtual System.Type[] GetAccepatbleTypesForB()
        {
            return null;
        }

        public virtual object GetDefaultConstValue()
        {
            throw new NotImplementedException();
        }

        public override string GetFailureMessage()
        {
            string str2;
            object[] objArray1 = new object[] { base.GetType().Name, " assertion failed.\n", base.go.name, ".", base.thisPropertyPath, " ", this.compareToType };
            string str = string.Concat(objArray1);
            switch (this.compareToType)
            {
                case CompareToType.CompareToObject:
                {
                    str2 = str;
                    object[] objArray2 = new object[] { str2, " (", this.other, ").", this.otherPropertyPath, " failed." };
                    str = string.Concat(objArray2);
                    break;
                }
                case CompareToType.CompareToConstantValue:
                {
                    str2 = str;
                    object[] objArray3 = new object[] { str2, " ", this.ConstValue, " failed." };
                    str = string.Concat(objArray3);
                    break;
                }
                case CompareToType.CompareToNull:
                    str = str + " failed.";
                    break;
            }
            str2 = str;
            object[] objArray4 = new object[] { str2, " Expected: ", this.m_ObjOtherVal, " Actual: ", base.m_ObjVal };
            return string.Concat(objArray4);
        }

        public virtual object ConstValue
        {
            [CompilerGenerated]
            get
            {
                return this.<ConstValue>k__BackingField;
            }
            [CompilerGenerated]
            set
            {
                this.<ConstValue>k__BackingField = value;
            }
        }

        public enum CompareToType
        {
            CompareToObject,
            CompareToConstantValue,
            CompareToNull
        }
    }
}

