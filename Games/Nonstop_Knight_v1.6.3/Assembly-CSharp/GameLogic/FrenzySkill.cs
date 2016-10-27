namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class FrenzySkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank)
        {
            <ExecuteRoutine>c__IteratorD1 rd = new <ExecuteRoutine>c__IteratorD1();
            rd.c = c;
            rd.<$>c = c;
            return rd;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorD1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CharacterInstance <$>c;
            internal Buff <buff>__0;
            internal CharacterInstance c;

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
                    Buff buff = new Buff();
                    buff.BaseStat1 = BaseStatProperty.AttacksPerSecond;
                    buff.Modifier = ConfigSkills.Frenzy.AttackSpeedMultiplier;
                    buff.DurationSeconds = ConfigSkills.Frenzy.Duration;
                    this.<buff>__0 = buff;
                    GameLogic.Binder.BuffSystem.startBuff(this.c, this.<buff>__0);
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

