namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MidasSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank)
        {
            return new <ExecuteRoutine>c__IteratorD5();
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD5 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal float <duration>__0;
            internal IEnumerator <ie>__1;

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
                        this.<duration>__0 = ConfigSkills.Midas.Duration;
                        this.<ie>__1 = TimeUtil.WaitForFixedSeconds(this.<duration>__0);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_007B;
                }
                if (this.<ie>__1.MoveNext())
                {
                    this.$current = this.<ie>__1.Current;
                    this.$PC = 1;
                    return true;
                }
                goto Label_007B;
                this.$PC = -1;
            Label_007B:
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

