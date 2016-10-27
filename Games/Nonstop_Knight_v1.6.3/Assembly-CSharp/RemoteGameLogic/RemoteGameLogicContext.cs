namespace RemoteGameLogic
{
    using App;
    using GameLogic;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class RemoteGameLogicContext : Context
    {
        [DebuggerHidden]
        protected override IEnumerator mapBindings(bool reallocateResources)
        {
            return new <mapBindings>c__Iterator1FF();
        }

        protected override void onCleanup()
        {
        }

        [CompilerGenerated]
        private sealed class <mapBindings>c__Iterator1FF : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;

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
                    GameLogic.Binder.GameState = new GameState();
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

