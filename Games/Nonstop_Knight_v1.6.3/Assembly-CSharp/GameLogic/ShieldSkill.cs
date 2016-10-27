namespace GameLogic
{
    using App;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ShieldSkill
    {
        [DebuggerHidden]
        public static IEnumerator ExecuteRoutine(CharacterInstance c, int rank)
        {
            <ExecuteRoutine>c__IteratorDA rda = new <ExecuteRoutine>c__IteratorDA();
            rda.c = c;
            rda.<$>c = c;
            return rda;
        }

        [CompilerGenerated]
        private sealed class <ExecuteRoutine>c__IteratorDA : IEnumerator, IDisposable, IEnumerator<object>
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
                    buff.BaseStat1 = BaseStatProperty.UniversalArmorBonus;
                    buff.Modifier = ConfigSkills.Shield.ArmorBonus;
                    buff.DurationSeconds = ConfigSkills.Shield.Duration;
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

