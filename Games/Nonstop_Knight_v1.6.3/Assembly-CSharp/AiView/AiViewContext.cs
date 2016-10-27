namespace AiView
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class AiViewContext : Context
    {
        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool allocatePersistentObjectPools)
        {
            <mapBindings>c__Iterator29 iterator = new <mapBindings>c__Iterator29();
            iterator.<>f__this = this;
            return iterator;
        }

        protected override void onCleanup()
        {
            AiView.Binder.AiBehaviourSystem = null;
        }

        protected void OnDisable()
        {
        }

        protected void OnEnable()
        {
        }

        [CompilerGenerated]
        private sealed class <mapBindings>c__Iterator29 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal AiViewContext <>f__this;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                this.$PC = -1;
                if (this.$PC == 0)
                {
                    AiView.Binder.AiBehaviourSystem = this.<>f__this.createPersistentGameObject<AiBehaviourSystem>(this.<>f__this.transform);
                }
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

