namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdSetCharacterGuardMode : ICommand
    {
        private CharacterInstance m_character;
        private bool m_guardEnabled;

        public CmdSetCharacterGuardMode(CharacterInstance character, bool guardEnabled)
        {
            this.m_character = character;
            this.m_guardEnabled = guardEnabled;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator65 iterator = new <executeRoutine>c__Iterator65();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance character, bool guardEnabled)
        {
            character.RemainInPlaceOnGuard = guardEnabled;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator65 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdSetCharacterGuardMode <>f__this;

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
                    CmdSetCharacterGuardMode.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_guardEnabled);
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

