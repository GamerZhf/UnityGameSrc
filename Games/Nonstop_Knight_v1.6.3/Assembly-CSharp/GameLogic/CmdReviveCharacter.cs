namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdReviveCharacter : ICommand
    {
        private CharacterInstance m_character;
        private float m_normalizedHpGain;

        public CmdReviveCharacter(CharacterInstance character, float normalizedHpGain)
        {
            this.m_character = character;
            this.m_normalizedHpGain = normalizedHpGain;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator5F iteratorf = new <executeRoutine>c__Iterator5F();
            iteratorf.<>f__this = this;
            return iteratorf;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator5F : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdReviveCharacter <>f__this;

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
                    this.<>f__this.m_character.resetDynamicRuntimeData();
                    this.<>f__this.m_character.PhysicsBody.Transform.position = this.<>f__this.m_character.PositionAtTimeOfDeath;
                    this.<>f__this.m_character.CurrentHp = this.<>f__this.m_character.MaxLife(true) * this.<>f__this.m_normalizedHpGain;
                    Binder.EventBus.CharacterRevived(this.<>f__this.m_character);
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

