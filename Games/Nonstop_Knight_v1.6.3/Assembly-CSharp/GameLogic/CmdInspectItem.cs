namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdInspectItem : ICommand
    {
        private ItemInstance m_itemInstance;

        public CmdInspectItem(ItemInstance itemInstance)
        {
            this.m_itemInstance = itemInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator78 iterator = new <executeRoutine>c__Iterator78();
            iterator.<>f__this = this;
            return iterator;
        }

        public static void ExecuteStatic(ItemInstance itemInstance)
        {
            itemInstance.InspectedByPlayer = true;
            Binder.EventBus.ItemInspected(itemInstance);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator78 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdInspectItem <>f__this;

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
                    CmdInspectItem.ExecuteStatic(this.<>f__this.m_itemInstance);
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

