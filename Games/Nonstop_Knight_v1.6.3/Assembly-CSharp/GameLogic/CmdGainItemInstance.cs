namespace GameLogic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ConsoleCommand("item")]
    public class CmdGainItemInstance : ICommand
    {
        private CharacterInstance m_character;
        private ItemInstance m_itemInstance;
        private string m_trackingId;

        public CmdGainItemInstance(string[] serialized)
        {
            this.m_character = Binder.GameState.Player.ActiveCharacter;
            string itemId = LangUtil.FirstLetterToUpper(serialized[0]);
            this.m_itemInstance = new ItemInstance(itemId, 1, 0, Binder.GameState.ActiveDungeon.Floor, this.m_character.OwningPlayer);
        }

        public CmdGainItemInstance(CharacterInstance character, ItemInstance itemInstance, [Optional, DefaultParameterValue("")] string trackingId)
        {
            this.m_character = character;
            this.m_itemInstance = itemInstance;
            this.m_trackingId = trackingId;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator8A iteratora = new <executeRoutine>c__Iterator8A();
            iteratora.<>f__this = this;
            return iteratora;
        }

        public static void ExecuteStatic(CharacterInstance character, ItemInstance itemInstance, [Optional, DefaultParameterValue("")] string trackingId)
        {
            character.getItemInstances(false).Add(itemInstance);
            Binder.EventBus.ItemGained(character, itemInstance, trackingId);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator8A : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdGainItemInstance <>f__this;

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
                    CmdGainItemInstance.ExecuteStatic(this.<>f__this.m_character, this.<>f__this.m_itemInstance, this.<>f__this.m_trackingId);
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

