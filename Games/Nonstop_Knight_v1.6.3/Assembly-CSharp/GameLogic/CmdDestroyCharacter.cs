namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdDestroyCharacter : ICommand
    {
        private CharacterInstance m_character;

        public CmdDestroyCharacter(CharacterInstance character)
        {
            this.m_character = character;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator53 iterator = new <executeRoutine>c__Iterator53();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(CharacterInstance c)
        {
            Binder.EventBus.CharacterPreDestroyed(c);
            if ((Binder.GameState.ActiveDungeon != null) && (Binder.GameState.ActiveDungeon.ActiveRoom != null))
            {
                Binder.GameState.ActiveDungeon.ActiveRoom.ActiveCharacters.Remove(c);
            }
            if (c.IsPersistent)
            {
                Binder.GameState.PersistentCharacters.Remove(c);
            }
            Binder.CharacterPool.returnObject(c);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator53 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdDestroyCharacter <>f__this;

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
                    CmdDestroyCharacter.ExecuteStatic(this.<>f__this.m_character);
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

