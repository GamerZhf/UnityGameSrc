namespace UnityTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class ActionBase : ScriptableObject
    {
        [CompilerGenerated]
        private static Func<FieldInfo, bool> <>f__am$cache4;
        public GameObject go;
        private MemberResolver m_MemberResolver;
        protected object m_ObjVal;
        public string thisPropertyPath = string.Empty;

        protected ActionBase()
        {
        }

        public bool Compare()
        {
            if (this.m_MemberResolver == null)
            {
                this.m_MemberResolver = new MemberResolver(this.go, this.thisPropertyPath);
            }
            this.m_ObjVal = this.m_MemberResolver.GetValue(this.UseCache);
            return this.Compare(this.m_ObjVal);
        }

        protected abstract bool Compare(object objVal);
        public ActionBase CreateCopy(GameObject oldGameObject, GameObject newGameObject)
        {
            ActionBase base2 = ScriptableObject.CreateInstance(base.GetType()) as ActionBase;
            IEnumerator<FieldInfo> enumerator = this.GetFields(base.GetType()).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    FieldInfo current = enumerator.Current;
                    object obj2 = current.GetValue(this);
                    if ((obj2 is GameObject) && ((obj2 as GameObject) == oldGameObject))
                    {
                        obj2 = newGameObject;
                    }
                    current.SetValue(base2, obj2);
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            return base2;
        }

        public virtual void Fail(AssertionComponent assertion)
        {
            Debug.LogException(new AssertionException(assertion), assertion.GetFailureReferenceObject());
        }

        public virtual System.Type[] GetAccepatbleTypesForA()
        {
            return null;
        }

        public virtual string GetConfigurationDescription()
        {
            string str = string.Empty;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = delegate (FieldInfo info) {
                    return info.FieldType.IsSerializable;
                };
            }
            IEnumerator<FieldInfo> enumerator = Enumerable.Where<FieldInfo>(base.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly), <>f__am$cache4).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object obj2 = enumerator.Current.GetValue(this);
                    if (obj2 is double)
                    {
                        obj2 = ((double) obj2).ToString("0.########");
                    }
                    if (obj2 is float)
                    {
                        obj2 = ((float) obj2).ToString("0.########");
                    }
                    str = str + obj2 + " ";
                }
            }
            finally
            {
                if (enumerator == null)
                {
                }
                enumerator.Dispose();
            }
            return str;
        }

        public virtual int GetDepthOfSearch()
        {
            return 2;
        }

        public virtual string[] GetExcludedFieldNames()
        {
            return new string[0];
        }

        public virtual string GetFailureMessage()
        {
            object[] objArray1 = new object[] { base.GetType().Name, " assertion failed.\n(", this.go, ").", this.thisPropertyPath, " failed. Value: ", this.m_ObjVal };
            return string.Concat(objArray1);
        }

        private IEnumerable<FieldInfo> GetFields(System.Type type)
        {
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        public virtual System.Type GetParameterType()
        {
            return typeof(object);
        }

        protected virtual bool UseCache
        {
            get
            {
                return false;
            }
        }
    }
}

