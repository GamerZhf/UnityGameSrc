namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdEvolveItem : ICommand
    {
        private CharacterInstance m_character;
        private ItemInstance m_itemInstance;

        public CmdEvolveItem(CharacterInstance character, ItemInstance itemInstance)
        {
            this.m_character = character;
            this.m_itemInstance = itemInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator9A iteratora = new <executeRoutine>c__Iterator9A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public static void ExecuteStatic(CharacterInstance character, ItemInstance itemInstance)
        {
            int num2 = itemInstance.EvolveRank + 1;
            itemInstance.EvolveRank = num2;
            Binder.EventBus.ItemEvolved(character, itemInstance);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator9A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdEvolveItem <>f__this;

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
                    CmdEvolveItem.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_itemInstance);
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

