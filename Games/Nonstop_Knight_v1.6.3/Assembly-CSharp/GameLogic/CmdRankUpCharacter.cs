namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdRankUpCharacter : ICommand
    {
        private CharacterInstance m_character;

        public CmdRankUpCharacter(CharacterInstance character)
        {
            this.m_character = character;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__IteratorAA raa = new <executeRoutine>c__IteratorAA();
            raa.<>f__this = this;
            return raa;
        }

        public static void ExecuteStatic(CharacterInstance characterInstance)
        {
            if (characterInstance.isAtMaxRank())
            {
                UnityEngine.Debug.LogWarning("CharacterInstance already at max rank, cannot rank-up: " + characterInstance.CharacterId);
            }
            else
            {
                int num = characterInstance.Rank + 1;
                characterInstance.Rank = num;
                Binder.EventBus.CharacterRankUpped(characterInstance);
            }
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__IteratorAA : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdRankUpCharacter <>f__this;

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
                    CmdRankUpCharacter.ExecuteStatic(this.<>f__this.m_character);
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

