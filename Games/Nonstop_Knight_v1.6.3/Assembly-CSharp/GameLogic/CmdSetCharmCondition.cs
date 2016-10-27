namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSetCharmCondition : ICommand
    {
        private CharacterInstance m_character;
        private bool m_charmed;

        public CmdSetCharmCondition(CharacterInstance character, bool charmed)
        {
            this.m_character = character;
            this.m_charmed = charmed;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator68 iterator = new <executeRoutine>c__Iterator68();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, bool charmed)
        {
            if (character.Charmed != charmed)
            {
                if (charmed)
                {
                    CmdInterruptCharacter.ExecuteStatic(character, true);
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(character, Vector3.zero, character.PhysicsBody.Transform.forward);
                }
                character.Charmed = charmed;
                Binder.EventBus.CharacterCharmConditionChanged(character);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator68 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetCharmCondition <>f__this;

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
                    CmdSetCharmCondition.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_charmed);
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

