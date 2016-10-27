namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdSetCharacterTarget : ICommand
    {
        private CharacterInstance m_character;
        private Vector3 m_manualTargetPos;
        private CharacterInstance m_targetCharacter;

        public CmdSetCharacterTarget(CharacterInstance character, CharacterInstance targetCharacter, Vector3 manualTargetPos)
        {
            this.m_character = character;
            this.m_targetCharacter = targetCharacter;
            this.m_manualTargetPos = manualTargetPos;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator66 iterator = new <executeRoutine>c__Iterator66();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, CharacterInstance targetCharacter, Vector3 manualTargetPos)
        {
            if ((character.TargetCharacter != targetCharacter) || (character.ManualTargetPos != manualTargetPos))
            {
                CharacterInstance oldTarget = character.TargetCharacter;
                character.TargetCharacter = targetCharacter;
                character.ManualTargetPos = manualTargetPos;
                Binder.EventBus.CharacterTargetUpdated(character, oldTarget);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator66 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetCharacterTarget <>f__this;

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
                    CmdSetCharacterTarget.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_targetCharacter, this.<>f__this.m_manualTargetPos);
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

