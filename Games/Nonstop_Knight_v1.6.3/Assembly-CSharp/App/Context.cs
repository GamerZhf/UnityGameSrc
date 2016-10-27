namespace App
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public abstract class Context : MonoBehaviourSingleton<Context>
    {
        [CompilerGenerated]
        private Transform <Tm>k__BackingField;
        protected List<IGenericObjectPool> m_persistentObjectPools = new List<IGenericObjectPool>();

        protected Context()
        {
        }

        protected void Awake()
        {
            if (MonoBehaviourSingleton<Context>.sm_instance == null)
            {
                UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
            }
            else
            {
                UnityEngine.Object.Destroy(base.gameObject);
                return;
            }
            this.Tm = base.transform;
            this.onAwake();
        }

        public void cleanup()
        {
            this.onCleanup();
        }

        protected T createPersistentGameObject<T>(Transform parentTm) where T: MonoBehaviour
        {
            GameObject target = new GameObject(typeof(T).ToString());
            target.transform.SetParent(parentTm);
            UnityEngine.Object.DontDestroyOnLoad(target);
            return target.AddComponent<T>();
        }

        protected T createPersistentGameObject<T>(GameObject preInstantiatedObject, Transform parentTm) where T: MonoBehaviour
        {
            preInstantiatedObject.transform.SetParent(parentTm);
            UnityEngine.Object.DontDestroyOnLoad(preInstantiatedObject);
            return preInstantiatedObject.GetComponent<T>();
        }

        protected void destroyPersistentGameObject<T>(T obj) where T: MonoBehaviour
        {
            if (obj != null)
            {
                UnityEngine.Object.Destroy(obj);
                UnityEngine.Object.Destroy(obj.gameObject);
            }
        }

        public void forceResetObjectPools()
        {
            for (int i = 0; i < this.m_persistentObjectPools.Count; i++)
            {
                this.m_persistentObjectPools[i].forceReturnAll();
            }
        }

        [DebuggerHidden]
        public IEnumerator initialize(bool allocateObjectPools)
        {
            <initialize>c__Iterator28 iterator = new <initialize>c__Iterator28();
            iterator.allocateObjectPools = allocateObjectPools;
            iterator.<$>allocateObjectPools = allocateObjectPools;
            iterator.<>f__this = this;
            return iterator;
        }

        protected abstract IEnumerator mapBindings(bool allocatePersistentObjectPools);
        protected virtual void onAwake()
        {
        }

        protected abstract void onCleanup();
        protected virtual void onInitialize()
        {
        }

        protected virtual void onPostInitialize()
        {
        }

        public void postInitialize()
        {
            this.onPostInitialize();
        }

        public Transform Tm
        {
            [CompilerGenerated]
            get
            {
                return this.<Tm>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Tm>k__BackingField = value;
            }
        }

        [CompilerGenerated]
        private sealed class <initialize>c__Iterator28 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal bool <$>allocateObjectPools;
            internal Context <>f__this;
            internal IEnumerator <ie>__0;
            internal bool allocateObjectPools;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<ie>__0 = this.<>f__this.mapBindings(this.allocateObjectPools);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_0081;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
                this.<>f__this.onInitialize();
                goto Label_0081;
                this.$PC = -1;
            Label_0081:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.$current;
                }
            }
        }
    }
}

