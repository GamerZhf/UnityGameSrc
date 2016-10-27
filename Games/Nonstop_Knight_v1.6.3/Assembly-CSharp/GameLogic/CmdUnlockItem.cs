namespace GameLogic
{
    using App;
    using Service;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class CmdUnlockItem : ICommand
    {
        private ItemInstance m_itemInstance;

        public CmdUnlockItem(ItemInstance itemInstance)
        {
            this.m_itemInstance = itemInstance;
        }

        [DebuggerHidden]
        public IEnumerator executeRoutine()
        {
            <executeRoutine>c__Iterator7D iteratord = new <executeRoutine>c__Iterator7D();
            iteratord.<>f__this = this;
            return iteratord;
        }

        public static void ExecuteStatic(ItemInstance itemInstance)
        {
            Player player = GameLogic.Binder.GameState.Player;
            CharacterInstance activeCharacter = player.ActiveCharacter;
            itemInstance.Unlocked = true;
            string selectedTournamentId = player.Tournaments.SelectedTournamentId;
            if ((itemInstance.Rarity == ConfigMeta.ITEM_HIGHEST_RARITY) && (selectedTournamentId != null))
            {
                Service.Binder.TournamentSystem.LogTournamentEvent(selectedTournamentId, TournamentLogEvent.LogEventType.PlayerGainedMaxRarityItem, true, itemInstance.ItemId);
            }
            GameLogic.Binder.EventBus.ItemUnlocked(activeCharacter, itemInstance);
        }

        public string[] serialize()
        {
            return null;
        }

        [CompilerGenerated]
        private sealed class <executeRoutine>c__Iterator7D : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal CmdUnlockItem <>f__this;

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
                    CmdUnlockItem.ExecuteStatic(this.<>f__this.m_itemInstance);
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

