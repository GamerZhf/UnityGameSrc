namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("heal")]
    public class CmdGainHp : ICommand
    {
        private double m_amount;
        private CharacterInstance m_character;
        private float m_delay;
        private bool m_silent;

        public CmdGainHp(string[] serialized)
        {
            this.m_character = Binder.GameState.ActiveDungeon.PrimaryPlayerCharacter;
            this.m_amount = int.Parse(serialized[0]);
        }

        public CmdGainHp(CharacterInstance character, double amount, bool silent, [Optional, DefaultParameterValue(0f)] float delay)
        {
            this.m_character = character;
            this.m_amount = amount;
            this.m_silent = silent;
            this.m_delay = delay;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator56 iterator = new <executeRoutine>c__Iterator56();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance c, double amount, bool silent)
        {
            c.CurrentHp = MathUtil.Clamp(c.CurrentHp + amount, 0.0, c.MaxLife(true));
            Binder.EventBus.CharacterHpGained(c, amount, silent);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator56 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainHp <>f__this;
            internal IEnumerator <ie>__0;

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
                        if (this.<>f__this.m_delay <= 0f)
                        {
                            goto Label_007E;
                        }
                        this.<ie>__0 = TimeUtil.WaitForUnscaledSeconds(this.<>f__this.m_delay);
                        break;

                    case 1:
                        break;

                    default:
                        goto Label_00B0;
                }
                if (this.<ie>__0.MoveNext())
                {
                    this.$current = this.<ie>__0.Current;
                    this.$PC = 1;
                    return true;
                }
            Label_007E:
                CmdGainHp.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_amount, this.<>f__this.m_silent);
                goto Label_00B0;
                this.$PC = -1;
            Label_00B0:
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

