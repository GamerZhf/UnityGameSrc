namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CmdGainShards : ICommand
    {
        private CharacterInstance m_character;
        private int m_count;
        private string m_itemId;

        public CmdGainShards(CharacterInstance character, string itemId, int count)
        {
            this.m_character = character;
            this.m_itemId = itemId;
            this.m_count = count;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator8B iteratorb = new <executeRoutine>c__Iterator8B();
            iteratorb.<>f__this = this;
            return iteratorb;
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator8B : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainShards <>f__this;

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
                    if (!this.<>f__this.m_character.Inventory.ItemShards.ContainsKey(this.<>f__this.m_itemId))
                    {
                        this.<>f__this.m_character.Inventory.ItemShards.Add(this.<>f__this.m_itemId, 0);
                    }
                    this.<>f__this.m_character.Inventory.ItemShards[this.<>f__this.m_itemId] = Mathf.Clamp(this.<>f__this.m_character.Inventory.ItemShards[this.<>f__this.m_itemId] + this.<>f__this.m_count, 0, 0x7fffffff);
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

