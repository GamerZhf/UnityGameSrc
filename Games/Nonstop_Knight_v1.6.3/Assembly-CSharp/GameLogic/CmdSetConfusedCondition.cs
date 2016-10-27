namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSetConfusedCondition : ICommand
    {
        private CharacterInstance m_character;
        private bool m_confused;

        public CmdSetConfusedCondition(CharacterInstance character, bool confused)
        {
            this.m_character = character;
            this.m_confused = confused;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator69 iterator = new <executeRoutine>c__Iterator69();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, bool confused)
        {
            if (character.Confused != confused)
            {
                if (confused)
                {
                    CmdInterruptCharacter.ExecuteStatic(character, true);
                    CmdSetCharacterVelocityAndFacing.ExecuteStatic(character, Vector3.zero, character.PhysicsBody.Transform.forward);
                }
                character.Confused = confused;
                Binder.EventBus.CharacterConfusedConditionChanged(character);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator69 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetConfusedCondition <>f__this;

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
                    CmdSetConfusedCondition.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_confused);
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

