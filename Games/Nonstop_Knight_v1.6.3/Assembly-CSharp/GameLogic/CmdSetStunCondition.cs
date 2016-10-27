namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSetStunCondition : ICommand
    {
        private CharacterInstance m_character;
        private bool m_stunned;

        public CmdSetStunCondition(CharacterInstance character, bool stunned)
        {
            this.m_character = character;
            this.m_stunned = stunned;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator6A iteratora = new <executeRoutine>c__Iterator6A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public static void ExecuteStatic(CharacterInstance character, bool stunned)
        {
            if (stunned)
            {
                character.StunnedCount++;
            }
            if (character.Stunned != stunned)
            {
                if (stunned)
                {
                    CmdInterruptCharacter.ExecuteStatic(character, true);
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(character, Vector3.zero, character.PhysicsBody.Transform.forward);
                }
                character.Stunned = stunned;
                Binder.EventBus.CharacterStunConditionChanged(character);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator6A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetStunCondition <>f__this;

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
                    CmdSetStunCondition.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_stunned);
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

